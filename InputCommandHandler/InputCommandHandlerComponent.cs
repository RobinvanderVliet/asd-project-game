using System;
using InputCommandHandler.antlr;
using InputCommandHandler.exception;
using Player.Model;
using Player.Services;

namespace InputCommandHandler
{
    public class InputCommandHandlerComponent
    {
        public void HandleCommands(IPlayerService playerService)
        {
            SendChat(GetCommand(), playerService);
        }

        private static void SendChat(string commando, IPlayerService playerService)
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