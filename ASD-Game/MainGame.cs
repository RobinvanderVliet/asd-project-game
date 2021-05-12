using Microsoft.Extensions.Logging;
using System;
using InputCommandHandler;
using Player.Model;
using Player.Services;
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

            public void Run()
            {
                Console.WriteLine("Game is gestart");

                // TODO: Remove from this method, team 2 will provide a command for it
                // AgentConfigurationService agentConfigurationService = new AgentConfigurationService();
                // agentConfigurationService.StartConfiguration();
                
                new WorldGeneration.Program();
                
                //moet later vervangen worden
                InputCommandHandlerComponent inputHandler = new InputCommandHandlerComponent();
                PlayerModel playerModel = new PlayerModel("Name", new Inventory(), new Bitcoin(20), new RadiationLevel(1));
                IPlayerService playerService = new PlayerService(playerModel); 
                Console.WriteLine("Type input messages below");
                while (true) // moet vervangen worden met variabele: isQuit 
                {
                    inputHandler.HandleCommands(playerService);
                }
            }
        }
    }
}
