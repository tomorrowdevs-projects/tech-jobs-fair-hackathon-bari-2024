using Fleck;
using Newtonsoft.Json;
using server.Dtos;
using server.Handlers;
using server.Models;

// Inizializzo un websocket
var WsServer = new WebSocketServer("ws://0.0.0.0:1402");
WsServer.Start(ws =>
{
    ws.OnOpen = () =>
    {
        Console.WriteLine("Connected client " + ws.ConnectionInfo.ClientIpAddress + ":" + ws.ConnectionInfo.ClientPort);
    };
    ws.OnMessage = message =>
    {
        try
        {
            Console.WriteLine("Received messsage: " + message.ToString());
            WsRequest? request = JsonConvert.DeserializeObject<WsRequest>(message);
            if (request == null) ws.Send(JsonConvert.SerializeObject(new WsResponse() { Event = "Unable to decode received message!", Status = "error" }));
            switch (request?.Event)
            {
                case "nuova_partita":
                    WebSocketHandler.JoinQueue(new Player
                    {
                        Name = request?.User ?? "Player " + WebSocketHandler.Players.Count,
                        WsConnection = ws
                    });
                    break;
                case "END":
                    // to do
                    // saluti il giocatore e chiudi la connessione
                    break;
                default:
                    ws.Send(JsonConvert.SerializeObject(new WsResponse() { Event = "Unknown command received", Status = "error" }));
                    break;
            }
        }
        catch (Exception ex)
        {
            ws.Send(JsonConvert.SerializeObject(new WsResponse() { Event = "Error parsing message", Status = "error" }));
            Console.WriteLine(ex.Message);
        }
    };
    ws.OnClose = () =>
    {
        //WebSocketHandler.WsConnections.Remove(ws);
        Console.WriteLine("Client disconnected " + ws.ConnectionInfo.ClientIpAddress + ":" + ws.ConnectionInfo.ClientPort);
    };
    ws.OnError = error =>
    {
        // salvo nei logs
        Console.WriteLine(error);
    };
});

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();
//}
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
