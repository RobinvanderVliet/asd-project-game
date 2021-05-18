using System.Runtime.Intrinsics.X86;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.TerrainTiles
{
    public class WaterTile : ITerrainTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public WaterTile(int x, int y)
        {
            Symbol = TileSymbol.WATER;
            IsAccessible = false;
            XPosition = x;
            YPosition = y;
        }
    }
}