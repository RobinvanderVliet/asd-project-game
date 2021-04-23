using System;
using Chat.antlr;
using Chat.exception;
using Player;

namespace Chat
{
    public class ChatComponent
    {
        public void HandleCommands(PlayerModel player)
        {
            SendChat(GetCommand(), player);
        }

        private static void SendChat(string commando, PlayerModel playerModel)
        {
            try
            {
                var pipeline = new Pipeline();
                pipeline.ParseCommand(commando);
                pipeline.transform(playerModel);
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