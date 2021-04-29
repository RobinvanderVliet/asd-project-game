using System;
using Chat.antlr;
using Chat.exception;
using Player.Model;
using Player.Services;

namespace Chat
{
    public class ChatComponent
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