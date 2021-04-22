using System;
using Chat.antlr;
using Chat.exception;

namespace Chat
{
    public class ChatComponent
    {
        public ChatComponent()
        {

        }
        public void HandleCommands()
        {
            SendChat(GetCommand());
        }

        private static void SendChat(string commando)
        {
            try
            {
                Pipeline pipeline = new Pipeline();
                pipeline.ParseCommand(commando);
            }
            catch (CommandSyntaxException e)
            {
                System.Console.WriteLine(e.Message);
            }


        }
        
        public String GetCommand()
        {
            return Console.ReadLine();
        }
    }
}
