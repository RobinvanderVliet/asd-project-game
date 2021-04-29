using Microsoft.Extensions.Logging;
using System;
using WorldGeneration;
using Player;
using Agent.Services;
using Chat;
using DatabaseHandler;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using WorldGeneration.Models;

namespace ASD_project
{
    partial class Program
    {
        public class MainGame : IMainGame
        {
            private readonly ILogger<MainGame> _log;

            public MainGame(ILogger<MainGame> log)
            {
                this._log = log;
            }

            public async void Run()
            {
                Console.WriteLine("Game is gestart");

                var repo = new ChunkRepository(null, new DbConnection(null));
                var services = new ChunkServices(null, repo);
                var chunk = new Chunk()
                {
                    X = 10,
                    Y = 10,
                    RowSize = 5,
                    Map = null
                };
                var wtf = await services.CreateAsync(chunk);
                var wtf1 = await services.ReadAsync(chunk);
                var wtt2 = await services.GetAllAsync();
                var wtf3 = await services.DeleteAllAsync();
                var wtf4 = await services.GetAllAsync();

                // TODO: Remove from this method, team 2 will provide a command for it
                //AgentConfigurationService agentConfigurationService = new AgentConfigurationService();
                //agentConfigurationService.StartConfiguration();
                
                //moet later vervangen worden
                // ChatComponent chat = new ChatComponent();
                // PlayerModel playerModel = new PlayerModel();
                // do
                // {
                //     chat.HandleCommands(playerModel);
                // } while (true); // moet vervangen worden met variabele: isQuit 
                //
                //new WorldGeneration.Program();
            }
        }
    }
}
