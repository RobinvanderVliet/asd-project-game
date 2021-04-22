/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project.
 
    This file is created by team: 2.
     
    Goal of this file: To be able to type commands and pass them to the parser.
     
*/

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

        private static void SendChat(string commando, PlayerModel player)
        {
            try
            {
                Pipeline pipeline = new Pipeline();
                pipeline.ParseCommand(commando);
                pipeline.transform(player);
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
