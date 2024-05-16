using System.Diagnostics;
using Newtonsoft.Json;
using server.Dtos;
using server.Models;

namespace server.Handlers
{
    public class WebSocketHandler
    {
        private const int MAX_PLAYERS = 2;
        private const int QUESTION_TIMEOUT = 15;
        private const int NUM_QUESTIONS = 10;

        public static List<Player> Players = [];
        public static GameRoom Rooms = new();
        public static QuestionList? QuestionRepo { get; set; }
        public static Question? QuestionSelected { get; set; }
        public static Stopwatch ElapsingTimer { get; set; } = new();


        public static void JoinQueue(Player player)
        {
            Players.Add(player);
            if (!Rooms.GameInProgress || Rooms.Players.Count <= MAX_PLAYERS)
            {
                player?.WsConnection?.Send(JsonConvert.SerializeObject(new WsResponse() { Event = "Accepted to play!", Status = "success" }));
                Rooms.Players.Add(player!);
                if (Rooms.Players.Count == MAX_PLAYERS)
                {
                    StartGame().ConfigureAwait(false);
                }
            }
            else
            {
                player?.WsConnection?.Send(JsonConvert.SerializeObject(new WsResponse() { Event = "Max number of player already joined!", Status = "warning" }));
                player?.WsConnection?.Close();
            }
        }


        private static async Task StartGame()
        {
            QuestionRepo = await GetQuestions();
            Rooms.GameInProgress = true;
            Players.ForEach(p => p.Score = 0);
            Players.ForEach(p => p.WsConnection?.Send(JsonConvert.SerializeObject(new WsResponse() { Event = "Game is starting!", Status = "success" })));
            PickQuestion();
            AskQuestion();
        }

        private static async Task<QuestionList?> GetQuestions()
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync("https://opentdb.com/api.php?amount=" + NUM_QUESTIONS + "&type=multiple");
            QuestionRepo = JsonConvert.DeserializeObject<QuestionList>(response);
            return QuestionRepo;
        }

        public static Question? PickQuestion()
        {
            if (QuestionRepo?.Questions.Count <= 2)
            {
                GetQuestions().ConfigureAwait(true);
                Thread.Sleep(1000);
            }
            // Create a Random object
            Random random = new();
            // Randomly select one item
            int index = random.Next((QuestionRepo?.Questions?.Count - 1) ?? 0);
            QuestionSelected = QuestionRepo?.Questions[index];
            QuestionRepo!.Questions.RemoveAt(index);

            return QuestionSelected;
        }

        public static void AskQuestion()
        {
            // Scelgo la domanda
            QuestionSelected = PickQuestion();
            Players.ForEach(p => p.WsConnection?.Send(QuestionSelected?.QuestionText));
            List<string> answers = [QuestionSelected?.CorrectAnswer ?? "", .. QuestionSelected?.IncorrectAnswers];
            Shuffle(answers);
            Console.WriteLine(JsonConvert.SerializeObject(answers));
            Players.ForEach(p => p.WsConnection?.Send(JsonConvert.SerializeObject(answers)));
            ElapsingTimer.Start();
            StartQuestionTimer(DateTime.Now.AddSeconds(QUESTION_TIMEOUT));
        }

        public async static void StartQuestionTimer(DateTime ExecutionTime)
        {
            await Task.Delay((int)ExecutionTime.Subtract(DateTime.Now).TotalMilliseconds);
            EndQuestion();
        }

        public static void Answer(String playername, String answer)
        {
            var player = Players.FirstOrDefault(p => p.Name == playername);
            if (player != null && !string.IsNullOrEmpty(answer) && QuestionSelected != null)
            {
                ElapsingTimer.Stop();
                player.Time += ElapsingTimer.Elapsed;
                player.Score += (answer == QuestionSelected.CorrectAnswer) ? 1 : 0;
            }
            Thread.Sleep(3000);
            PickQuestion();
            AskQuestion();
        }

        public static void PlayerDisconnected(Fleck.IWebSocketConnection connection)
        {
            Players.RemoveAll(p => p.WsConnection == connection);
            Rooms.GameInProgress = false;
        }

        private static void EndQuestion()
        {
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

        private static void EndGame()
        {
            Rooms.GameInProgress = false;
            var finalResults = Players.OrderByDescending(p => p.Score).ToList();
            Players.ForEach(async p => await p!.WsConnection!.Send(JsonConvert.SerializeObject(finalResults)));
        }


        public static void SendToPlayer(Player player, WsResponse response)
        {
            player.WsConnection?.Send(JsonConvert.SerializeObject(response));
        }
        public static void SendBroadcast(WsResponse response)
        {
            foreach (var player in Players)
            {
                player.WsConnection?.Send(JsonConvert.SerializeObject(response));
            }
        }


        private static void Shuffle<T>(List<T> list)
        {
            Random random = new();
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                // Swap list[i] with the element at random index
                (list[j], list[i]) = (list[i], list[j]);
            }
        }

    }
}