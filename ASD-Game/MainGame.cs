using System;
using ActionHandling;
using Chat;
using InputHandling;
using Microsoft.Extensions.Logging;
using Network;
using Session;
using UserInterface;
using WorldGeneration;

namespace ASD_project
{
    partial class Program
    {
        public class MainGame : IMainGame
        {
            private const bool DEBUG_INTERFACE = false; //TODO: remove when UI is complete, obviously
            private IInputHandler _inputHandler;
            private IScreenHandler _screenHandler;

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
                            _inputHandler.HandleConfigurationScreenCommands();
                        }
                        else if (currentScreen is GameScreen)
                        {
                            _inputHandler.HandleGameScreenCommands();
                        }
                        else if (currentScreen is LobbyScreen) 
                        {
                            _inputHandler.HandleLobbyScreenCommands();
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