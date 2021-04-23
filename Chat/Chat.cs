using System;
using Chat.antlr;
using Chat.exception;
using Player.Model;

namespace Chat
{
    public class ChatComponent
    {
        public void HandleCommands(IPlayerModel player)
        {
            SendChat(GetCommand(), player);
        }

        private static void SendChat(string commando, IPlayerModel playerModel)
        {
            try
            {
                var pipeline = new Pipeline();
                pipeline.ParseCommand(commando);
                pipeline.Transform(playerModel);
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