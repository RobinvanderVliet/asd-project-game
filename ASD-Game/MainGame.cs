using Microsoft.Extensions.Logging;
using System;
using InputCommandHandler;
using Player.Model;
using Player.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Agent.Mapper;
using Agent.Models;
using WorldGeneration;
using Player;
using Agent.Services;
using Chat;
using Session;

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

            public MainGame(ILogger<MainGame> log, IInventory inventory, IChatHandler chatHandler, ISessionHandler sessionHandler)
            {
                _log = log;
                _inventory = inventory;
                _chatHandler = chatHandler;
                _sessionHandler = sessionHandler;
            }

            public void Run()
            {
                Console.WriteLine("Game is gestart");
                InputCommandHandlerComponent inputHandler = new InputCommandHandlerComponent();

                // TODO: Remove from this method, a command needs to be made
                // AgentConfigurationService agentConfigurationService = new AgentConfigurationService(new List<Configuration>(), new FileToDictionaryMapper(), inputHandler);
                // agentConfigurationService.Configure();
                
                // new WorldGeneration.Program();
                
                //moet later vervangen worden
                PlayerModel playerModel = new PlayerModel("Name", _inventory, new Bitcoin(20), new RadiationLevel(1));
                IPlayerService playerService = new PlayerService(playerModel, _chatHandler, _sessionHandler); 
                Console.WriteLine("Type input messages below");
                while (true) // moet vervangen worden met variabele: isQuit 
                {
                    inputHandler.HandleCommands(playerService);
                }
            }
        }
    }
}
