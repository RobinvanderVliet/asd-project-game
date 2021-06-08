using System;
using System.Diagnostics.CodeAnalysis;
using System.Timers;
using InputHandling.Antlr;
using InputHandling.Exceptions;
using Messages;
using Session;
using Session.GameConfiguration;
using UserInterface;
using WebSocketSharp;

namespace InputHandling
{
    public class InputHandler : IInputHandler
    {
        private IPipeline _pipeline;
        private ISessionHandler _sessionHandler;
        private IScreenHandler _screenHandler;
        private IGameConfigurationHandler _gameConfigurationHandler;
        private IMessageService _messageService;
        private static Timer aTimer;
        private const string RETURN_KEYWORD = "return";
        private string _enteredSessionName;

        public string START_COMMAND = "start_session";

        public InputHandler(IPipeline pipeline, ISessionHandler sessionHandler, IScreenHandler screenHandler, IMessageService messageService, IGameConfigurationHandler gameConfigurationHandler)
        {
            _pipeline = pipeline;
            _sessionHandler = sessionHandler;
            _screenHandler = screenHandler;
            _gameConfigurationHandler = gameConfigurationHandler;
            _messageService = messageService;
        }

        public InputHandler()
        {
            //Empty constructor needed for testing purposes
        }

        public void HandleGameScreenCommands()
        {
            SendCommand(GetCommand());
            _screenHandler.RedrawGameInputBox();
        }

        // Needed to handle input on client when host has started session
        public void HandleGameScreenCommands(string input)
        {
            SendCommand(input);
            _screenHandler.RedrawGameInputBox();
        }

        private void SendCommand(string commando)
        {
            try
            {
                _pipeline.ParseCommand(commando);
                _pipeline.Transform();
            }
            catch (Exception e)
            {
                _messageService.AddMessage(e.Message);
            }
        }

        public virtual string GetCommand()
        {
            return _screenHandler.GetScreenInput();
        }
        
        public void HandleStartScreenCommands()
        {
            var input = GetCommand();
            var option = 0;
            int.TryParse(input, out option);
            
            switch (option)
            {
                case 1:
                    _screenHandler.TransitionTo(new ConfigurationScreen());
                    break;
                case 2:
                    _sessionHandler.RequestSessions();
                    _screenHandler.TransitionTo(new SessionScreen());
                    break;
                case 3:
                    _screenHandler.TransitionTo(new LoadScreen());
                    break;
                case 4:
                    _screenHandler.TransitionTo(new EditorScreen());
                    break;
                case 5:
                    SendCommand("exit");
                    break;
                default:
                    StartScreen startScreen = _screenHandler.Screen as StartScreen;
                    startScreen.UpdateInputMessage("Not a valid option, try again!");
                    break;
            }
        }

        public void HandleSessionScreenCommands()
        {

            SessionScreen sessionScreen = _screenHandler.Screen as SessionScreen;
            var input = GetCommand();
            
            if (input == RETURN_KEYWORD)
            {
                _screenHandler.TransitionTo(new StartScreen());
                return;
            }
            
            var inputParts = input.Split(" ");

            if (inputParts.Length != 2)
            {
                sessionScreen.UpdateInputMessage("Provide both a session number and username (example: 1 Gerrit)");
            }
            else
            {
                int sessionNumber = 0;
                int.TryParse(input[0].ToString(), out sessionNumber);

                string sessionId = sessionScreen.GetSessionIdByVisualNumber(sessionNumber - 1);
        
                if (sessionId.IsNullOrEmpty())
                {
                    sessionScreen.UpdateInputMessage("Not a valid session, try again!");
                }
                else
                {
                    _screenHandler.TransitionTo(new LobbyScreen());
                    SendCommand("join_session \"" + sessionId + "\" \"" + inputParts[1].Replace("\"", "") + "\"");
                }
            }
        }

        public void HandleLobbyScreenCommands()
        {
            var input = GetCommand();
            // Needed to handle input on client when host has started session
            if (_screenHandler.Screen is GameScreen)
            {
                HandleGameScreenCommands(input);
            }
            else
            {
                if (input == RETURN_KEYWORD)
                {
                    _screenHandler.TransitionTo(new StartScreen());
                    return;
                }

                if (input == START_COMMAND)
                {
                    SendCommand(START_COMMAND);
                }

                if (input.Contains("SAY"))
                {
                    SendCommand(input);
                }
                else if (input.Contains("SHOUT"))
                {
                    SendCommand(input);
                }
               
            }

        }
        public void HandleConfigurationScreenCommands()
        {
            var input = GetCommand();
            if (input == RETURN_KEYWORD)
            {
                _screenHandler.TransitionTo(new StartScreen());
                _gameConfigurationHandler.SetGameConfiguration();
            }
            else
            {
                bool configurationCompleted = _gameConfigurationHandler.HandleAnswer(input);

                if (configurationCompleted)
                {
                    _gameConfigurationHandler.SetGameConfiguration();
                    _sessionHandler.CreateSession(_gameConfigurationHandler.GetSessionName(), _gameConfigurationHandler.GetUsername());
                    _screenHandler.TransitionTo(new LobbyScreen());
                }
            }
        }
    }
}