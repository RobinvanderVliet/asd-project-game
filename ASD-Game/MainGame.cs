using Microsoft.Extensions.Logging;
using System;
using InputCommandHandler;
using Player.Model;
using Player.Services;

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
                InputCommandHandlerComponent chat = new InputCommandHandlerComponent();
                PlayerModel playerModel = new PlayerModel("Name", new Inventory(), new Bitcoin(20), new RadiationLevel(1));
                IPlayerService playerService = new PlayerService(playerModel);
                do
                {
                    chat.HandleCommands(playerService);
                } while (true); // moet vervangen worden met variabele: isQuit 
            }
        }
    }
}