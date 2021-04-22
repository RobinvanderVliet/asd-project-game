namespace WorldGeneration.Models.BuildingTiles
{
    public class HouseTile : BuildingTile
    {
        public HouseTile()
        {
            Symbol = TileSymbol.House;
            IsAccessible = true;
        }

        public override string DrawBuilding()
        {
            throw new System.NotImplementedException();
        }
    }
}
