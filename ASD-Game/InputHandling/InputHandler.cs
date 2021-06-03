using System;
using System.Diagnostics.CodeAnalysis;
using System.Timers;
using InputHandling.Antlr;
using InputHandling.Exceptions;
using Messages;
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
        private IMessageService _messageService;
        private static Timer aTimer;
        private const string RETURN_KEYWORD = "return";

        public InputHandler(IPipeline pipeline, ISessionHandler sessionHandler, IScreenHandler screenHandler, IMessageService messageService)
        {
            _pipeline = pipeline;
            _sessionHandler = sessionHandler;
            _screenHandler = screenHandler;
            _messageService = messageService;
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
            catch (Exception e)
            {
                _messageService.AddMessage(e.Message);
            }
        }

        public virtual string GetCommand()
        {
            return Console.ReadLine();
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
                    Environment.Exit(0);
                    break;
                default:
                    StartScreen startScreen = _screenHandler.Screen as StartScreen;
                    startScreen.UpdateInputMessage("Not a valid option, try again!");
                    break;
            }
        }

        public void HandleSessionScreenCommands()
        {
            var input = GetCommand();
            int sessionNumber = 0;
            int.TryParse(input, out sessionNumber);
            SessionScreen sessionScreen = _screenHandler.Screen as SessionScreen;

            if (input == RETURN_KEYWORD)
            {
                _screenHandler.TransitionTo(new StartScreen());
            }
            else if (sessionNumber > 0)
            {
                string sessionId = sessionScreen.GetSessionIdByVisualNumber(sessionNumber - 1);

                if (sessionId.IsNullOrEmpty())
                {
                    sessionScreen.UpdateInputMessage("Not a valid session, try again!");
                }
                else
                {
                    SendCommand("join_session \"" + sessionId + "\"");
                    _screenHandler.TransitionTo(new ConfigurationScreen()); // maybe a waiting screen in stead?   
                }
            }
            else
            {
                sessionScreen.UpdateInputMessage("That is not a number, please try again!");
            }
        }
    }
}