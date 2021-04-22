using System;
using Chat.antlr;
using Chat.exception;
using Player;

namespace Chat
{
    public class ChatComponent
    {
        public ChatComponent()
        {

        }
        public void HandleCommands(PlayerModel player)
        {
            SendChat(GetCommand(), player);
        }

        private static void SendChat(string commando, Player.PlayerModel playerModel)
        {
            try
            {
                Pipeline pipeline = new Pipeline();
                pipeline.ParseCommand(commando);
                pipeline.transform(playerModel);
            }
            catch (CommandSyntaxException e)
            {
                System.Console.WriteLine(e.Message);
            }



        }
        
        public string GetCommand()
        {
            return Console.ReadLine();
        }
    }
}
