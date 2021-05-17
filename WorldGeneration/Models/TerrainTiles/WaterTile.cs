using System.Runtime.Intrinsics.X86;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.TerrainTiles
{
    public class WaterTile : ITerrainTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public WaterTile(int x, int y)
        {
            Symbol = TileSymbol.WATER;
            IsAccessible = false;
            X = x;
            Y = y;
        }
    }
}