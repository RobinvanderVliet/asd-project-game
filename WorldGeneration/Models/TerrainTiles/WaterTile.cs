namespace WorldGeneration.Models.TerrainTiles
{
    public class WaterTile : TerrainTile
    {
        public WaterTile() 
        {
            Symbol = TileSymbol.Water;
            IsAccessible = false;
        }
    }
}
