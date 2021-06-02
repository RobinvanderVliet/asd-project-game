using System;
using System.Diagnostics.CodeAnalysis;
using System.Timers;
using InputHandling.Antlr;
using InputHandling.Exceptions;
using Session;
using UserInterface;
using WebSocketSharp;

namespace InputHandling
{
    public class InputHandler : IInputHandler
    {
        private IPipeline _pipeline;
        private ISessionHandler _sessionHandler;
        private IScreenHandler _screenHandler;
        private const string RETURN_KEYWORD = "return";
        private string _enteredSessionName;

        public InputHandler(IPipeline pipeline, ISessionHandler sessionHandler, IScreenHandler screenHandler)
        {
            _pipeline = pipeline;
            _sessionHandler = sessionHandler;
            _screenHandler = screenHandler;
        }

        public InputHandler()
        {

        }

        public void HandleGameScreenCommands()
        {
            SendCommand(GetCommand());
        }
        private void SendCommand(string commando)
        {
            try
            {
                _pipeline.ParseCommand(commando);
                _pipeline.Transform();
            }
            catch (CommandSyntaxException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (MoveException e)
            {
                Console.WriteLine(e.Message);
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
                    SendCommand("join_session \"" + sessionId + "\" \"" + inputParts[1].Replace("\"", "") + "\"");
                    _screenHandler.TransitionTo(new ConfigurationScreen());
                }
            }
        }
    }
}