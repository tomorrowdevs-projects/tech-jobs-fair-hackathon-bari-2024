using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using server.Handlers;
using server.Services;
using Fleck;
using FleckLogLevel = Fleck.LogLevel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });

        services.AddSingleton<QuizServer>();
        services.AddSingleton<WebSocketHandler>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseCors("CorsPolicy");

        FleckLog.Level = LogLevel.Debug;
        var allSockets = new List<IWebSocketConnection>();
        var server = new WebSocketServer("ws://0.0.0.0:1402");
        server.Start(socket =>
        {
            socket.OnOpen = () =>
            {
                Console.WriteLine("Open!");
                WebSocketHandler.WsConnections.Add(socket);
            };
            socket.OnClose = () =>
            {
                Console.WriteLine("Close!");
                WebSocketHandler.WsConnections.Remove(socket);
            };
            socket.OnMessage = message =>
            {
                Console.WriteLine(message);
                var request = JsonConvert.DeserializeObject<WsRequest>(message);
                WebSocketHandler.ProcessMessageReceived(request);
            };
        });
    }
}