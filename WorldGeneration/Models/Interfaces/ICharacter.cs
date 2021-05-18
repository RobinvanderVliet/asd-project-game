using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.Models.Interfaces
{
    public interface ICharacter
    {
        string Symbol { get; set; }
        int XPosition { get; set; }
        int YPosition { get; set; }
    }
}
