using System.Collections.Generic;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models
{
    public class Character : ICharacter
    {
        public string Symbol { get; set; }
        public int PlayerX { get; set; }
        public int PlayerY { get; set; }
    }
}