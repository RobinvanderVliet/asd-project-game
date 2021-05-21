using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models
{
    public abstract class Tile : ITile
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public string Symbol { get; set; }
        public bool IsAccessible { get; set; }
    }
}