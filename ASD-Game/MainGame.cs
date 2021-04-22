using Chat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using Player;

namespace ASD_project
{
    partial class Program
    {
        public class MainGame : IMainGame
        {
            private readonly ILogger<MainGame> log;

            public MainGame(ILogger<MainGame> log)
            {
                this.log = log;
            }

            public void Run()
            {
                Console.WriteLine("Game is gestart");

                //moet later vervangen worden
                ChatComponent chat = new ChatComponent();
                PlayerModel player = new PlayerModel();
                do
                {
                    chat.HandleCommands(player); // speler model meegeven
                } while (true); // moet vervangen worden met variabele: isQuit 
            }
        }
    }
}