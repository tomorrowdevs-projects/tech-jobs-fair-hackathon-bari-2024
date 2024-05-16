using Fleck;
using Newtonsoft.Json;
using server.Dtos;

namespace server.Handlers
{
    public class WebSocketHandler
    {
        public static List<IWebSocketConnection> WsConnections { get; set; } = [];

        public static void ProcessMessageReceived(WsRequest? request)
        {
            if (request == null) Send(new WsResponse() { Event = "NULL message received!", Status = "error" });
            Console.WriteLine("Event:" + request?.Event + " - Status: " + request?.Status);
            switch (request?.Event)
            {
                case "nuova_partita":
                
                    break;
                case "END":
                    // to do
                    // saluti il giocatore e chiudi la connessione
                    break;
            }

        }

        public static void Send(WsResponse response)
        {
            foreach (var webSocketConnection in WsConnections)
            {
                webSocketConnection.Send(JsonConvert.SerializeObject(response));
            }
        }

    }
}