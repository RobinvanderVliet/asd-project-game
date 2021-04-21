using Microsoft.Extensions.Logging;
using System;

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
                new WorldGeneration.Program();
                Console.WriteLine("Game is gestart");
            }
        }
    }
}
