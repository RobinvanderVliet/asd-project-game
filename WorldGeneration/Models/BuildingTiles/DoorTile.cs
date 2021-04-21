namespace WorldGeneration.Models.BuildingTiles
{
    public class DoorTile : BuildingTile
    {
        public DoorTile() 
        {
            Symbol = "/";
            Accessible = true;
        }

        public override void DrawBuilding()
        {
            throw new System.NotImplementedException();
        }
    }
}
