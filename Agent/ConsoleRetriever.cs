using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent
{
    public class ConsoleRetriever
    {
        public virtual String GetConsoleLine()
        {
            return Console.ReadLine();
        }
    }
}
