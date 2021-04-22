using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.BuildingTiles
{
    public class DoorTile : BuildingTile, ITile
    {
        public DoorTile() 
        {
            Symbol = TileSymbol.Door;
            IsAccessible = true;
        }

        public override string DrawBuilding()
        {
            throw new System.NotImplementedException();
        }
    }
}
