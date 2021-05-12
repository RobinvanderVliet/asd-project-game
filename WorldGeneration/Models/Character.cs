using System.Collections.Generic;

namespace WorldGeneration.Models
{
    public abstract class Character : ICharacter
    {
        public string Symbol { get; set; }
        public int[] CurrentPosition { get; set; }
    }
}