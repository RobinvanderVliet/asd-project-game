using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World.Models
{
    [ExcludeFromCodeCoverage]
    public class Chunk
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ITile[] Map { get; set; }
        public int RowSize { get; set; }
        public int Seed { get; set; }

        public Chunk()
        {
        }

        public Chunk(int x, int y, ITile[] map, int rowSize)
        {
            X = x;
            Y = y;
            Map = map;
            RowSize = rowSize;
        }
    }
}