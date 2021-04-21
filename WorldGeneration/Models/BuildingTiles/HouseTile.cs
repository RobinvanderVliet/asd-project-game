namespace WorldGeneration.Models.BuildingTiles
{
    public class HouseTile : BuildingTile
    {
        public HouseTile() 
        {
            Symbol = "+";
            Accessible = true;
        }

        public override void DrawBuilding()
        {
            throw new System.NotImplementedException();
        }
    }
}
