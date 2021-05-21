using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent
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
