using System.Collections.Generic;

namespace WorldGeneration.Models
{
    public abstract class Character : ICharacter
    {
        public int[] CurrentPosition { get; set; }
    }
}