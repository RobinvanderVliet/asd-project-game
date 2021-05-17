using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.Models.Interfaces
{
    public interface ICharacter
    {
        public string Symbol { get; set; }
        public int[] CurrentPosition { get; set; }
    }
}
