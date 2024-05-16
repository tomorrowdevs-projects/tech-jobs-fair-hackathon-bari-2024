using Fleck;

namespace server.Models
{
    public class GameRoom
    {
        public List<Player> Players { get; set; } = [];
        public Boolean GameInProgress { get; set; } = false;
    }
}