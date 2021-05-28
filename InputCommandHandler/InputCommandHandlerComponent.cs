using System;
using InputCommandHandler.Antlrr;
using InputCommandHandler.Exceptions;
using Player.Services;
using Session;

namespace InputCommandHandler
{
    public class InputCommandHandlerComponent
    {
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
            catch (Exception e) //CommandSyntaxException,MoveException,SlotException
            {
                Console.WriteLine(e.Message);
            }
        }

        public virtual string GetCommand()
        {
            return Console.ReadLine();
        }
    }
}