using Newtonsoft.Json;
using server.Models;
using Timer = System.Timers.Timer;
using System.Diagnostics;

namespace server.Services
{
    public class QuizServer
    {
        private const int MAX_PLAYERS = 2;
        private const int QUESTION_TIMEOUT = 15000; // 15 seconds
        private const int NUM_QUESTIONS = 10;

        private List<Player> Players = [];
        private QuestionList? Questions = new();
        public Question? CurrentQuestion;
        private Timer questionTimer = new();
        private Stopwatch ElapsingTimer = new();
        private bool gameInProgress = false;


        public async Task Join(Player player)
        {
            if (player == null) return;
            if (gameInProgress || Players.Count >= MAX_PLAYERS)
            {
                await player?.WsConnection?.Send("Max number of player already joined!");
                return;
            }
            Players.Add(player);
            await player?.WsConnection?.Send("Accepted to play!");
        }

        public async Task StartGame()
        {
            gameInProgress = true;
            Players.ForEach(p => p.Score = 0);
            Questions = await GetQuestions();
            Players.ForEach(p => p.WsConnection?.Send("Game is starting!"));
            PickQuestion();
            AskQuestion();
        }

        public async Task<QuestionList?> GetQuestions()
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync("https://opentdb.com/api.php?amount=" + NUM_QUESTIONS + "&type=multiple");
            Questions = JsonConvert.DeserializeObject<QuestionList>(response);
            return Questions;
        }

        public Question? PickQuestion()
        {
            // Create a Random object
            Random random = new();
            // Randomly select one item
            int index = random.Next((Questions?.Questions?.Length - 1) ?? 0);
            CurrentQuestion = Questions?.Questions[index];
            while (CurrentQuestion?.Asked == true)
            {
                index = random.Next(Questions?.Questions?.Length ?? 0);
                CurrentQuestion = Questions?.Questions[index];
            }
            Questions.Questions[index].Asked = true;
            return CurrentQuestion;
        }

        public void AskQuestion()
        {
            // Scelgo la domanda
            CurrentQuestion = PickQuestion();
            Players.ForEach(p => p.WsConnection?.Send(CurrentQuestion?.QuestionText));
            List<string> answers = [CurrentQuestion?.CorrectAnswer ?? "", .. CurrentQuestion?.IncorrectAnswers];
            Shuffle(answers);
            Players.ForEach(p => p.WsConnection?.Send(JsonConvert.SerializeObject(answers)));
            questionTimer = new Timer(QUESTION_TIMEOUT);
            questionTimer.Elapsed += (sender, e) => EndQuestion();
            questionTimer.AutoReset = false;
            questionTimer.Start();
            ElapsingTimer.Start();
        }

        public void Answer(String playername, String answer)
        {
            var player = Players.FirstOrDefault(p => p.Name == playername);
            if (player != null && !string.IsNullOrEmpty(answer) && CurrentQuestion != null)
            {
                questionTimer.Stop();
                ElapsingTimer.Stop();
                player.Time += ElapsingTimer.Elapsed;
                player.Score += (answer == CurrentQuestion.CorrectAnswer) ? 1 : 0;
            }
            Thread.Sleep(3000);
            PickQuestion();
            AskQuestion();
        }

        public void PlayerDisconnected(Fleck.IWebSocketConnection connection)
        {
            Players.RemoveAll(p => p.WsConnection == connection);
            gameInProgress = false;
        }

        private void EndQuestion()
        {
            questionTimer.Stop();
            ElapsingTimer.Stop();
            Players.ForEach(p => p.Time += ElapsingTimer.Elapsed);
            if (Players.Count >= 2)
            {
                Thread.Sleep(3000);
                PickQuestion();
                AskQuestion();
            }
            else
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            gameInProgress = false;
            var finalResults = Players.OrderByDescending(p => p.Score).ToList();
            Players.ForEach(async p => await p?.WsConnection?.Send(JsonConvert.SerializeObject(finalResults)));
        }


        static void Shuffle<T>(List<T> list)
        {
            Random random = new Random();
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                // Swap list[i] with the element at random index
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}