using System.Diagnostics;
using Fleck;
using Newtonsoft.Json;
using server.Dtos;
using server.Models;

namespace server.Handlers
{
    public class WebSocketHandler
    {
        private const int MAX_PLAYERS = 2;
        private const int QUESTION_TIMEOUT = 15000; // 15 seconds
        private const int NUM_QUESTIONS = 10;

        public static List<Player> Players = [];
        public static QuestionList? QuestionRepo { get; set; }
        public static Question? QuestionSelected { get; set; }
        public static Timer QuestionTimer { get; set; } = new Timer(ExpiredTime);
        public static Stopwatch ElapsingTimer { get; set; } = new();
        public static Boolean GameInProgress { get; set; } = false;


        public async Task Join(Player player)
        {
            if (player == null) return;
            if (GameInProgress || Players.Count >= MAX_PLAYERS)
            {
                await player?.WsConnection?.Send("Max number of player already joined!");
                return;
            }
            Players.Add(player);
            await player?.WsConnection?.Send("Accepted to play!");
        }

        public static void SendTpPlayer(Player player, WsResponse response)
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

        private static void ExpiredTime(object? state)
        {
            throw new NotImplementedException();
        }
    }
}