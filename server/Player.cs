namespace server.Models
{
    public class Player
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public string Answer { get; set; }
        public DateTime Time { get; set; }
        public int Score { get; set; }
    }
}