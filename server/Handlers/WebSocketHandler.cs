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
            if (request == null) Send(new WsResponse() { Message = "NULL message received!" });

            Console.WriteLine(request?.Message);
            switch (request?.Message)
            {
                case "START":
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