namespace WorldGeneration.Models.BuildingTiles
{
    public class WallTile : BuildingTile
    {
        public WallTile() 
        {
            Symbol = TileSymbol.Wall;
            IsAccessible = false;
        }

        public override string DrawBuilding()
        {
            throw new System.NotImplementedException();
        }
    }
}
