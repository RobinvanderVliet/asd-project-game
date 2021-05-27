using Chat;
using InputCommandHandler;
using Microsoft.Extensions.Logging;
using Network;
using Player.ActionHandlers;
using Player.Model;
using Player.Services;
using Session;
using System;
using WorldGeneration;

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
            private readonly IGameSessionHandler _gameSessionHandler;
            private readonly IClientController _clientController;
            private readonly IWorldService _worldService;

            public MainGame(ILogger<MainGame> log, IInventory inventory, IClientController clientController, IWorldService worldService,
                IChatHandler chatHandler, ISessionHandler sessionHandler, IMoveHandler moveHandler, IGameSessionHandler gameSessionHandler)
            {
                _log = log;
                _inventory = inventory;
                _clientController = clientController;
                _worldService = worldService;
                _chatHandler = chatHandler;
                _sessionHandler = sessionHandler;
                _moveHandler = moveHandler;
                _gameSessionHandler = gameSessionHandler;
            }

            public void Run()
            {
                Console.WriteLine("Game is gestart");
                InputCommandHandlerComponent inputHandler = new InputCommandHandlerComponent();

                // TODO: Remove from this method, a command needs to be made
                // AgentConfigurationService agentConfigurationService = new AgentConfigurationService(new List<Configuration>(), new FileToDictionaryMapper(), inputHandler);
                // agentConfigurationService.Configure();

                //moet later vervangen worden
                IPlayerModel playerModel = new PlayerModel("Gerard", _inventory, new Bitcoin(20), new RadiationLevel(1));
                IPlayerService playerService = new PlayerService(playerModel, _chatHandler, _moveHandler, _clientController, _worldService);

                ISessionService sessionService = new SessionService(_sessionHandler, _gameSessionHandler);
                // moet vervangen worden met variabele: isRun
                while (true)
                {
                    Console.WriteLine("Type input messages below");
                    inputHandler.HandleCommands(playerService, sessionService);
                }
            }
        }
    }
}