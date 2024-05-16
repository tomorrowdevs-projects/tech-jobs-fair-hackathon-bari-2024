using Fleck;
using Newtonsoft.Json;
using server.Dtos;
using server.Handlers;

// Inizializzo un websocket
var WsServer = new WebSocketServer("ws://0.0.0.0:1402");
WsServer.Start(ws =>
{
    ws.OnOpen = () =>
    {
        WebSocketHandler.WsConnections.Add(ws);
        Console.WriteLine("Connected client " + ws.ConnectionInfo.ClientIpAddress + ":" + ws.ConnectionInfo.ClientPort);
    };
    ws.OnMessage = message =>
    {
        try
        {
            Console.WriteLine("Received messsage: " + message.ToString());
            WsRequest? msg = JsonConvert.DeserializeObject<WsRequest>(message);
            WebSocketHandler.ProcessMessageReceived(msg);
        }
        catch (Exception ex)
        {
            // inviare al client: non sono riuscito a decodificare il messaggio
            Console.WriteLine(ex.Message);
        }
    };
    ws.OnClose = () =>
    {
        WebSocketHandler.WsConnections.Remove(ws);
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
app.UseCors(options => options.AllowAnyOrigin()
                            //.WithOrigins("http://localhost:4200/")
                            .AllowAnyMethod()
                            .AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
