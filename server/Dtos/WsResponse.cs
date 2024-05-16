namespace server.Dtos
{
    public class WsResponse
    {
        public String Event { get; set; } = String.Empty;
        public String Status { get; set; } = String.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}