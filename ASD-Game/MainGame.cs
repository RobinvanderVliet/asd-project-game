using System;
using InputHandling;
using UserInterface;

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
            private readonly IGamesSessionService _gamesSessionService;
            private const bool DEBUG_INTERFACE = true; //TODO: remove when UI is complete, obviously
            private IInputHandler _inputHandler;
            private IScreenHandler _screenHandler;

            public MainGame(ILogger<MainGame> log, IInventory inventory, IClientController clientController, IWorldService worldService, 
                IChatHandler chatHandler, ISessionHandler sessionHandler, IMoveHandler moveHandler, IGameSessionHandler gameSessionHandler
                , IGamesSessionService gamesSessionService)
            {
                _log = log;
                _inventory = inventory;
                _clientController = clientController;
                _worldService = worldService;
                _chatHandler = chatHandler;
                _sessionHandler = sessionHandler;
                _moveHandler = moveHandler;
                _gameSessionHandler = gameSessionHandler;
                _gamesSessionService = gamesSessionService;


            public MainGame(IInputHandler inputHandler, IScreenHandler screenHandler)
            {
                _screenHandler = screenHandler;
                _inputHandler = inputHandler;
            }

            public void Run()
            {

                if (!DEBUG_INTERFACE)
                {
                    _screenHandler.TransitionTo(new StartScreen());
                    while (true)
                    {
                        var currentScreen = _screenHandler.Screen;

                        if (currentScreen is StartScreen)
                        {
                            _inputHandler.HandleStartScreenCommands();
                        }
                        else if (currentScreen is SessionScreen)
                        {
                            _inputHandler.HandleSessionScreenCommands();
                        }
                        else if (currentScreen is ConfigurationScreen)
                        {
                        }
                        else if (currentScreen is WaitingScreen)
                        {
                        }
                        else if (currentScreen is GameScreen)
                        {
                            _inputHandler.HandleGameScreenCommands();
                        }
                    }
                }
                else
                {
                    while (true)
                    {
                        Console.WriteLine("Type input messages below");
                        _inputHandler.HandleGameScreenCommands();
                    }
                }
            }
        }
    }
}