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
using WorldGeneration.Models.Interfaces;

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

                var chunk = new Chunk()
                {
                    X = 10,
                    Y = 10,
                    RowSize = 5,
                    Map = Array.Empty<ITile>()
                };
                
                using var db = new DbConnection().GetConnection();
                var result = db.GetCollection<Chunk>("Chunks").Insert(chunk);

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
