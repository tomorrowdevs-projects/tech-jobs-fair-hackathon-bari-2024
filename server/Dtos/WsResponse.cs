namespace server.Dtos
{
    public class WsResponse
    {
        public String Message { get; set; } = String.Empty;
        public Object? Content { get; set; }
    }
}