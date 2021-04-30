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
                
                //moet later vervangen worden
                // InputCommandHandlerComponent inputHandler = new InputCommandHandlerComponent();
                // PlayerModel playerModel = new PlayerModel("Name", new Inventory(), new Bitcoin(20), new RadiationLevel(1));
                // IPlayerService playerService = new PlayerService(playerModel); 
                // while (true) // moet vervangen worden met variabele: isQuit 
                // {
                //     inputHandler.HandleCommands(playerService);
                // }
                
                new WorldGeneration.Program();
            }
        }
    }
}
