using System;
using ActionHandling;
using InputHandling;
using UserInterface;

namespace ASD_project
{
    partial class Program
    {
        public class MainGame : IMainGame
        {
            private const bool DEBUG_INTERFACE = true; //TODO: remove when UI is complete, obviously
            private IInputHandler _inputHandler;
            private IScreenHandler _screenHandler;
            private IRelativeStatHandler _relativeStatHandler;

            public MainGame(IInputHandler inputHandler, IScreenHandler screenHandler,IRelativeStatHandler relativeStatHandler)
            {
                _screenHandler = screenHandler;
                _inputHandler = inputHandler;
                _relativeStatHandler = relativeStatHandler;
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