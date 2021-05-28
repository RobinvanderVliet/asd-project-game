using DataTransfer.Model.World.Interfaces;

namespace DataTransfer.Model.World
{
    public abstract class Tile : ITile
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public string Symbol { get; set; }
        public bool IsAccessible { get; set; }
    }
}