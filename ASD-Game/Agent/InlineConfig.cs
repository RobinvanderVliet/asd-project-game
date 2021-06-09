using System;
using System.IO;

namespace ASD_Game.Agent
{
    class InlineConfig
    {
        public void setup()
        {
            var filePath = Console.ReadLine();

            Console.Clear();

            var lines = File.ReadLines(filePath);

            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
