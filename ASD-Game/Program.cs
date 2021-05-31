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
                    services.AddScoped<IWorldService, WorldService>();
                    services.AddScoped<IGameSessionHandler, GameSessionHandler>();
                    services.AddSingleton<IDbConnection, DbConnection>();
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped(typeof(IServicesDb<>), typeof(ServicesDb<>));
                    services.AddScoped<IScreenHandler, ScreenHandler>();
                    services.AddScoped<IInputHandler, InputHandler>();
                    services.AddScoped<IPipeline, Pipeline>();
                    services.AddScoped<IEvaluator, Evaluator>();
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
