using ASD_project.World.Models.Interfaces;

namespace ASD_project.World.Services
{
    public interface IItemService
    {
        public ITile PutItemOnTile(ITile tile, float noiseResult);
    }
}