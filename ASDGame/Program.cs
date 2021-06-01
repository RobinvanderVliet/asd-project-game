using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using DatabaseHandler;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using WorldGeneration;
using ActionHandling;
using Chat;
using InputHandling;
using InputHandling.Antlr;
using InputHandling.Antlr.Transformer;
using Network;
using Session;
using UserInterface;

namespace ASD_project
{
    partial class Program
    {
        //Setup logger and injection
        private static void Main(string[] args)
        {
            INetworkComponent networkComponent = new NetworkComponent();
            IWorldService worldService = new WorldService();
            IScreenHandler screenHandler = new ScreenHandler();
            IClientController clientController = new ClientController(networkComponent);
            IChatHandler chatHandler = new ChatHandler(clientController);
            ISessionHandler sessionHandler = new SessionHandler(clientController, screenHandler);
            IMoveHandler moveHandler = new MoveHandler(clientController, worldService);
            IGameSessionHandler gameSessionHandler = new GameSessionHandler(clientController, worldService, sessionHandler);
            IEvaluator evaluator = new Evaluator(sessionHandler, moveHandler, gameSessionHandler, chatHandler);
            IPipeline pipeline = new Pipeline(evaluator);
            MainGame mainGame = new MainGame(new InputHandler(pipeline, sessionHandler, screenHandler), new ScreenHandler());

            mainGame.Run();
        }

        private static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json", optional: true);
            //.AddEnvironmentVariables();
        }
    }
}