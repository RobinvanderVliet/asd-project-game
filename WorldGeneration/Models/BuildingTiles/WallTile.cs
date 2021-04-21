namespace WorldGeneration.Models.BuildingTiles
{
    public class WallTile : BuildingTile
    {
        public WallTile() 
        {
            Symbol = "\u25A0";
            Accessible = false;
        }

        public override void DrawBuilding()
        {
            throw new System.NotImplementedException();
        }
    }
}
