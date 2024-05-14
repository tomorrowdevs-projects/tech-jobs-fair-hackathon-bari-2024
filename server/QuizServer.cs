using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using server.Models;
using Timer1 = System.Timers.Timer;

namespace server.Services
{
    public class QuizServer
    {
        private List<Player> players = new List<Player>();
        private Question currentQuestion;
        private Timer questionTimer;
        private bool gameInProgress = false;
        private readonly WebSocketHandler _webSocketHandler;

        private const int MAX_PLAYERS = 10;
        private const int QUESTION_TIMEOUT = 15000; // 15 seconds
        private const int NUM_QUESTIONS = 10;

        public QuizServer(WebSocketHandler webSocketHandler)
        {
            _webSocketHandler = webSocketHandler;
        }

        public async Task Join(string playerName, string connectionId)
        {
            if (gameInProgress || players.Count >= MAX_PLAYERS)
            {
                await _webSocketHandler.Send(new WsResponse { Message = "Wait" });
                return;
            }

            players.Add(new Player { ConnectionId = connectionId, Name = playerName });
            await _webSocketHandler.Send(new WsResponse { Message = "Joined" });
        }

        public async Task StartGame()
        {
            gameInProgress = true;
            players.ForEach(p => p.Score = 0);
            await _webSocketHandler.Send(new WsResponse { Message = "GameStart" });

            await GetNextQuestion();
        }

        public async Task Answer(string connectionId, string answer)
        {
            var player = players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player != null && !string.IsNullOrEmpty(answer) && currentQuestion != null)
            {
                player.Answer = answer;
                player.Time = DateTime.Now;
                player.Score += (answer == currentQuestion.CorrectAnswer) ? 1 : 0;
            }
        }

        public void PlayerDisconnected(string connectionId)
        {
            players.RemoveAll(p => p.ConnectionId == connectionId);
        }

        private async Task GetNextQuestion()
        {
            var question = await GetRandomQuestion();
            currentQuestion = new Question
            {
                QuestionText = question["question"].ToString(),
                CorrectAnswer = question["correct_answer"].ToString(),
                AllAnswers = ((JArray)question["incorrect_answers"]).Select(a => a.ToString()).ToList()
            };

            await _webSocketHandler.Send(new WsResponse { Message = "NewQuestion", Data = currentQuestion.QuestionText });

            questionTimer = new Timer(QUESTION_TIMEOUT);
            questionTimer.Elapsed += async (sender, e) => await EndQuestion();
            questionTimer.AutoReset = false;
            questionTimer.Start();
        }

        private async Task<JObject> GetRandomQuestion()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("https://opentdb.com/api.php?amount=1&type=multiple");
                return JObject.Parse(response)["results"][0] as JObject;
            }
        }

        private async Task EndQuestion()
        {
            questionTimer.Stop();
            var results = players.Select(p => new { p.Name, Correct = p.Answer == currentQuestion.CorrectAnswer, Time = p.Time }).ToList();
            await _webSocketHandler.Send(new WsResponse { Message = "QuestionEnd", Data = results });

            if (players.Count >= 2)
            {
                await GetNextQuestion();
            }
            else
            {
                await EndGame();
            }
        }

        private async Task EndGame()
        {
            gameInProgress = false;
            var finalResults = players.OrderByDescending(p => p.Score).ToList();
            await _webSocketHandler.Send(new WsResponse { Message = "GameEnd", Data = finalResults });
        }
    }
}