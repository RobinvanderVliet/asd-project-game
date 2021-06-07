using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using ActionHandling;
using ASD_project.ActionHandling;
using ASD_project.Chat;
using ASD_project.DatabaseHandler;
using ASD_project.DatabaseHandler.Repository;
using ASD_project.DatabaseHandler.Services;
using ASD_project.InputHandling;
using ASD_project.InputHandling.Antlr;
using ASD_project.InputHandling.Antlr.Transformer;
using ASD_project.Items.Services;
using ASD_project.Messages;
using ASD_project.Network;
using ASD_project.Session;
using ASD_project.Session.GameConfiguration;
using ASD_project.UserInterface;
using ASD_project.World.Services;
using Messages;
using Session.GameConfiguration;

namespace ASD_project
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            
            Log.Logger.Information("Application starting");
            
            //Example of dependency injection with GreetingService
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IMainGame, MainGame>();
                    services.AddScoped<INetworkComponent, NetworkComponent>();
                    services.AddScoped<IClientController, ClientController>();
                    services.AddScoped<IInventoryHandler, InventoryHandler>();
                    services.AddScoped<IChatHandler, ChatHandler>();
                    services.AddScoped<ISessionHandler, SessionHandler>();
                    services.AddScoped<IMoveHandler, MoveHandler>();
                    services.AddScoped<IRelativeStatHandler, RelativeStatHandler>();
                    services.AddScoped<IWorldService, WorldService>();
                    services.AddScoped<IMessageService, MessageService>();
                    services.AddScoped<IGameSessionHandler, GameSessionHandler>();
                    services.AddSingleton<IDBConnection, DBConnection>();
                    services.AddScoped<IItemService, ItemService>();
                    services.AddScoped<ISpawnHandler, SpawnHandler>();
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped(typeof(IDatabaseService<>), typeof(DatabaseService<>));
                    services.AddScoped<IScreenHandler, ScreenHandler>();
                    services.AddScoped<IInputHandler, InputHandler>();
                    services.AddScoped<IPipeline, Pipeline>();
                    services.AddScoped<IEvaluator, Evaluator>();
                    services.AddScoped<IGameConfigurationHandler, GameConfigurationHandler>();
                })
                .UseSerilog()
                .Build();
            
            var svc = ActivatorUtilities.CreateInstance<MainGame>(host.Services);
            svc.Run();
        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();

        }
    }
}
