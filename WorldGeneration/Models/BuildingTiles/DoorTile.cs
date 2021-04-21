namespace WorldGeneration.Models.BuildingTiles
{
    public class DoorTile : BuildingTile
    {
        public DoorTile() 
        {
            Symbol = TileSymbol.Door;
            IsAccessible = true;
        }

        public override void DrawBuilding()
        {
            throw new System.NotImplementedException();
        }
    }
}
