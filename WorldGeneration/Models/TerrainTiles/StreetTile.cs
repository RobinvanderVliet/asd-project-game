using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.TerrainTiles
{
    public class StreetTile : ITerrainTile
    {
        
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public StreetTile()
        {
            Symbol = TileSymbol.STREET;
            IsAccessible = true;
        }
    }
}