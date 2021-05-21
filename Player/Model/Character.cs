using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models
{
    public class Character : ICharacter
    {
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
    }
}