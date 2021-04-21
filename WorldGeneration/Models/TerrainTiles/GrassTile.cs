namespace WorldGeneration.Models.TerrainTiles
{
    public class GrassTile : TerrainTile
    {
        public GrassTile() 
        {
            Symbol = TileSymbol.Grass;
            IsAccessible = true;
        }
    }
}
