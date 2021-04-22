using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models
{
    public abstract class BuildingTile : Tile, IBuildingTile
    {
        public abstract string DrawBuilding();
    }
}
