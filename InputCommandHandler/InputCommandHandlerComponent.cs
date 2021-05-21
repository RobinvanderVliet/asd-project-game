using System;
using InputCommandHandler.Antlrr;
using InputCommandHandler.Exceptions;
using Player.Services;

namespace InputCommandHandler
{
    public class InputCommandHandlerComponent
    {
        public void HandleCommands(IPlayerService playerService)
        {
            SendCommand(GetCommand(), playerService);
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

        public virtual string GetCommand()
        {
            return Console.ReadLine();
        }
    }
}