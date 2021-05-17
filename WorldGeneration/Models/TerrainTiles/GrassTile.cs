using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.TerrainTiles
{
    public class GrassTile : ITerrainTile
    {
        
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public GrassTile(int x, int y)
        {
            Symbol = TileSymbol.GRASS;
            IsAccessible = true;
            X = x;
            Y = y;
        }
    }
}