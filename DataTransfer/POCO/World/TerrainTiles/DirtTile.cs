using DataTransfer.POCO.World.Interfaces;

namespace DataTransfer.POCO.World.TerrainTiles
{
    public class DirtTile : ITerrainTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public DirtTile(int x, int y)
        {
            Symbol = TileSymbol.DIRT;
            IsAccessible = true;
            XPosition = x;
            YPosition = y;
        }
    }
}