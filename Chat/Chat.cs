/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project.
 
    This file is created by team: 2.
     
    Goal of this file: To be able to type commands and pass them to the parser.
     
*/

using System;
using Chat.antlr;

namespace Chat
{
    public class ChatComponent
    {
        public ChatComponent()
        {

        }
        public void handleCommands()
        {
            sendChat(getCommand());
        }

        private static void sendChat(string commando)
        {
            Pipeline pipeline = new Pipeline();
            pipeline.parseCommando(commando);
        }
        
        public String getCommand()
        {
            return Console.ReadLine();
        }
    }
}
