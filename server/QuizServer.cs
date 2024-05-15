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
        private Question? CurrentQuestion;
        private Timer questionTimer;
        private Stopwatch ElapsingTimer;
        private bool gameInProgress = false;


        public QuizServer()
        { }

        public async Task Join(Player player)
        {
            if (player == null) return;
            if (gameInProgress || Players.Count >= MAX_PLAYERS)
            {
                await player.WsConnection?.Send("Max number of player already joined!");
                return;
            }
            Players.Add(player);
            await player.WsConnection?.Send("Accepted to play!");
        }

        public async Task StartGame()
        {
            gameInProgress = true;
            Players.ForEach(p => p.Score = 0);
            Questions = await GetQuestions();
            Players.ForEach(p => p.WsConnection?.Send("Game is starting!"));
            await PickQuestion();
            await AskQuestion();
        }

        private async Task<QuestionList?> GetQuestions()
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync("https://opentdb.com/api.php?amount=" + NUM_QUESTIONS + "&type=multiple");
            var Questions = JsonConvert.DeserializeObject<QuestionList>(response);
            return Questions;
        }

        private async Task<Question?> PickQuestion()
        {
            // Create a Random object
            Random random = new();
            // Randomly select one item
            int index = random.Next(Questions?.Questions?.Length ?? 0);
            Question selectedItem = Questions?.Questions[index];
            while (selectedItem.Asked == true)
            {
                index = random.Next(Questions?.Questions?.Length ?? 0);
                selectedItem = Questions?.Questions[index];
            }
            Questions.Questions[index].Asked = true;
            return selectedItem;
        }

        public async Task AskQuestion()
        {
            // Scelgo la domanda
            CurrentQuestion = await PickQuestion();
            Players.ForEach(p => p.WsConnection?.Send(CurrentQuestion.QuestionText));
            List<string> answers = new List<string> { CurrentQuestion?.CorrectAnswer ?? "" };
            answers.AddRange(CurrentQuestion?.IncorrectAnswers);
            Shuffle(answers);
            Players.ForEach(p => p.WsConnection?.Send(JsonConvert.SerializeObject(answers)));
            questionTimer = new Timer(QUESTION_TIMEOUT);
            questionTimer.Elapsed += async (sender, e) => await EndQuestion();
            questionTimer.AutoReset = false;
            questionTimer.Start();
            ElapsingTimer.Start();
        }

        public async Task Answer(String playername, String answer)
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
            await PickQuestion();
            await AskQuestion();
        }

        public void PlayerDisconnected(Fleck.IWebSocketConnection connection)
        {
            Players.RemoveAll(p => p.WsConnection == connection);
            gameInProgress = false;
        }


        private async Task EndQuestion()
        {
            questionTimer.Stop();
            ElapsingTimer.Stop();
            Players.ForEach(p => p.Time += ElapsingTimer.Elapsed);
            if (Players.Count >= 2)
            {
                Thread.Sleep(3000);
                await PickQuestion();
                await AskQuestion();
            }
            else
            {
                await EndGame();
            }
        }

        private async Task EndGame()
        {
            gameInProgress = false;
            var finalResults = Players.OrderByDescending(p => p.Score).ToList();
            Players.ForEach(async p => await p.WsConnection?.Send(JsonConvert.SerializeObject(finalResults)));
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