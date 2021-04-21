namespace WorldGeneration.Models.TerrainTiles
{
    public class DirtTile : TerrainTile
    {
        public DirtTile() 
        {
            Symbol = TileSymbol.Dirt;
            IsAccessible = true;
        }
    }
}
