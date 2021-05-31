using InputCommandHandler.Antlr;
using InputCommandHandler.Exceptions;
using Player.Services;
using Session;
using System;

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
    }
}