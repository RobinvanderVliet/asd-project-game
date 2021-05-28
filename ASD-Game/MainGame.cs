using InputCommandHandler;
using Player.Services;
using UserInterface;

namespace ASD_project
{
    partial class Program
    {
        public class MainGame : IMainGame
        {
            private IInputCommandHandlerComponent _inputHandler;
            private IScreenHandler _screenHandler;
            private IPlayerService _playerService;

            public MainGame(IInputCommandHandlerComponent inputHandler, IScreenHandler screenHandler, IPlayerService playerService)
            {
                _screenHandler = screenHandler;
                _inputHandler = inputHandler;
                _playerService = playerService;
            }
            public void Run()
            {
                _playerService.SetupPlayer();
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
                    else if (currentScreen is EditorScreen)
                    {
                        _inputHandler.HandleEditorScreenCommands();
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
        }
    }
}