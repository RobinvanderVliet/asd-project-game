using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using WorldGeneration;

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
                // Note this code is for testing purposes only!
                Console.WriteLine("Game is gestart");
                   Class1 whatever = new Class1();
                    Console.WriteLine(whatever.prototype("hallo"));
            
            }
        }
    }
}
