/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project.
 
    This file is created by team: 2.
     
    Goal of this file: To be able to type commands and pass them to the parser.
     
*/

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
