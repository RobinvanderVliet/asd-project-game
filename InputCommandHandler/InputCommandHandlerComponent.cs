using System;
using System.Timers;
using InputCommandHandler.Antlrr;
using InputCommandHandler.Exceptions;
using Player.Services;
using Session;
using Session.DTO;
using UserInterface;
using WebSocketSharp;

namespace InputCommandHandler
{
    public class InputCommandHandlerComponent : IInputCommandHandlerComponent
    {
        private IPlayerService _playerService;
        private ISessionService _sessionService;
        private IScreenHandler _screenHandler;
        private static Timer aTimer;
        private const string RETURN_KEYWORD = "return";

        public InputCommandHandlerComponent(IPlayerService playerService, ISessionService sessionService, IScreenHandler screenHandler)
        {
            _playerService = playerService;
            _sessionService = sessionService;
            _screenHandler = screenHandler;
        }
        public void HandleGameScreenCommands()
        {
            SendCommand(GetCommand());
        }
        private void SendCommand(string commando)
        {
            try
            {
                var pipeline = new Pipeline(_playerService, _sessionService);
                pipeline.ParseCommand(commando);
                pipeline.Transform();
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
                    _sessionService.RequestSessions();
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

        public void HandleConfigurationScreenCommands()
        {
            var input = GetCommand();
            if (input == RETURN_KEYWORD)
            {
                _screenHandler.TransitionTo(new StartScreen());
            }
        }
    }
}