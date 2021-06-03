using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
    public interface IItemService
    {
        public ITile PutItemOnTile(ITile tile, float noiseResult);
    }
}