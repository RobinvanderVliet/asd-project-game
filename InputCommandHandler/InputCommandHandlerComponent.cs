using System;
using InputCommandHandler.Antlrr;
using InputCommandHandler.Exceptions;
using Player.Services;
using Session;

namespace InputCommandHandler
{
    public class InputCommandHandlerComponent
    {
        public void HandleCommands(IPlayerService playerService)
        {
            SendCommand(GetCommand(), playerService);
        }

        public void HandleSession(ISessionService sessionService)
        {
            SendCommandSession(GetCommand(), sessionService);
        }

        private static void SendCommandSession(string commando, ISessionService sessionService)
        {
            try
            {
                var pipeline = new Pipeline();
                pipeline.ParseCommand(commando);
                pipeline.Transform(sessionService); // Transform commando to join/create session;
            }
            catch (CommandSyntaxException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void SendCommand(string commando, IPlayerService playerService)
        {
            try
            {
                var pipeline = new Pipeline();
                pipeline.ParseCommand(commando);
                pipeline.Transform(playerService);
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

        public string GetCommand()
        {
            return Console.ReadLine();
        }
    }
}