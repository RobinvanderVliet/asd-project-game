using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models
{
    public abstract class Tile : ITile
    {
        public int X { get; set; }
        public int Y { get; set; }

        public string Symbol { get; set; }
        public bool Accessible { get; set; }

        public Tile()
        {
        }

    }
}
