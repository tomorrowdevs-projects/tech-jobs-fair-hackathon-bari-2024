using Fleck;

namespace server.Models
{
    public class Player
    {
        public IWebSocketConnection? WsConnection { get; set; } = null;
        public String Name { get; set; } = "Player_" + DateTime.Now.Ticks;
        public TimeSpan Time { get; set; } = TimeSpan.Zero;
        public Double Score { get; set; }
    }
}