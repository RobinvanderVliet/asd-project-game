using Microsoft.Extensions.Logging;
using System;
using InputCommandHandler;
using Player.Model;
using Player.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using WorldGeneration;
using Player;
using Agent.Services;
using Chat;
using Session;
using Player.ActionHandlers;
using Player.DTO;

namespace ASD_project
{
    partial class Program
    {
        public class MainGame : IMainGame
        {
            private readonly ILogger<MainGame> _log;
            private readonly IInventory _inventory;
            private readonly IChatHandler _chatHandler;
            private readonly ISessionHandler _sessionHandler;
            private readonly IMoveHandler _moveHandler;

            public MainGame(ILogger<MainGame> log, IInventory inventory, IChatHandler chatHandler, ISessionHandler sessionHandler, IMoveHandler moveHandler)
            {
                this._log = log;
                _inventory = inventory;
                _chatHandler = chatHandler;
                _sessionHandler = sessionHandler;
                _moveHandler = moveHandler;
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
                PlayerModel playerModel = new PlayerModel("Name",  _inventory, new Bitcoin(20), new RadiationLevel(1));
                //lobby start
                //networkcomponent heeft lijst van players
                //die players moeten toegevoegd worden aan playerPositions
                List<PlayerDTO> playerPositions = new List<PlayerDTO>
                {
                    new PlayerDTO("Joe", 10, 10),
                    new PlayerDTO("Mama", 40, 40)
                };
                IPlayerService playerService = new PlayerService(playerModel, _chatHandler, _sessionHandler, _moveHandler); 
                Console.WriteLine("Type input messages below");
                while (true) // moet vervangen worden met variabele: isQuit 
                {
                    inputHandler.HandleCommands(playerService);
                }
            }
        }
    }
}
