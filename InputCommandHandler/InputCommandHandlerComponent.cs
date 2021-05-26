using System;
using InputCommandHandler.Antlrr;
using InputCommandHandler.Exceptions;
using Player.Services;
using Session;
using UserInterface;

namespace InputCommandHandler
{
    public class InputCommandHandlerComponent : IInputCommandHandlerComponent
    {
        private IPlayerService _playerService;
        private ISessionService _sessionService;
        public void HandleCommands(IPlayerService playerService, ISessionService sessionService)
        {
            SendCommand(GetCommand(), playerService, sessionService);
        }
        private static void SendCommand(string commando, IPlayerService playerService, ISessionService sessionService)
        {
            try
            {
                var pipeline = new Pipeline();
                pipeline.ParseCommand(commando);
                pipeline.Transform(playerService, sessionService);
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

        public void HandleStartScreenCommands(IScreenHandler screenHandler, ISessionService sessionService)
        {
            var input = Console.ReadLine();
            int option;
            int.TryParse(input, out option);
            
            switch (option)
            {
                case 1:
                    screenHandler.TransitionTo(new ConfigurationScreen());
                    break;
                case 2:
                    screenHandler.TransitionTo(new SessionScreen());
                    sessionService.RequestSessions();
                    break;
                case 3:
                    screenHandler.TransitionTo(new EditorScreen());
                    break;
                case 4:
                    screenHandler.TransitionTo(new HelpScreen());
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}