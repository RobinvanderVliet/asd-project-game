using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ActionHandling;
using Agent.Mapper;
using Agent.Services;
using ASD_Game.ActionHandling;
using ASD_Game.Agent.Mapper;
using ASD_Game.Agent.Services;
using ASD_Game.Chat;
using ASD_Game.DatabaseHandler;
using ASD_Game.DatabaseHandler.Repository;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.InputHandling;
using ASD_Game.InputHandling.Antlr;
using ASD_Game.InputHandling.Antlr.Transformer;
using ASD_Game.Items.Services;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Session;
using ASD_Game.Session.GameConfiguration;
using ASD_Game.UserInterface;
using ASD_Game.World.Models.Characters.Algorithms.Creator;
using ASD_Game.World;
using ASD_Game.World.Services;
using Creature;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Session;

namespace ASD_Game
{
    [ExcludeFromCodeCoverage]
    partial class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);
            
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
                    services.AddScoped<IAttackHandler, AttackHandler>();
                    services.AddScoped<IRelativeStatHandler, RelativeStatHandler>();
                    services.AddScoped<IWorldService, WorldService>();
                    services.AddScoped<IMessageService, MessageService>();
                    services.AddScoped<IGameSessionHandler, GameSessionHandler>();
                    services.AddSingleton<IDbConnection, DbConnection>();
                    services.AddScoped<IItemService, ItemService>();
                    services.AddScoped<ISpawnHandler, SpawnHandler>();
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped(typeof(IDatabaseService<>), typeof(DatabaseService<>));
                    services.AddScoped<IGamesSessionService, GamesSessionService>();
                    services.AddScoped<IScreenHandler, ScreenHandler>();
                    services.AddScoped<IInputHandler, InputHandler>();
                    services.AddScoped<IAgentCreator, AgentCreator>();
                    services.AddScoped<IAgentHandler, AgentHandler>();
                    services.AddScoped<IPipeline, Pipeline>();
                    services.AddScoped<IRandomItemGenerator, RandomItemGenerator>();
                    services.AddScoped<IEvaluator, Evaluator>();
                    services.AddScoped<IConfigurationService, ConfigurationService>();
                    services.AddScoped<IFileToConfigurationMapper, FileToConfigurationMapper>();
                    services.AddScoped<IGameConfigurationHandler, GameConfigurationHandler>();
                })
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
