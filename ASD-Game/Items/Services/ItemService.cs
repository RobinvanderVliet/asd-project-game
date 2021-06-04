using ActionHandling;
using ASD_project.World;
using ASD_project.World.Models.Interfaces;
using ASD_project.World.Services;
using Items;
using MathNet.Numerics.Optimization;

namespace ASD_project.Items.Services
{
    public class ItemService : IItemService
    {
        private ISpawnHandler _spawnHandler;
        public ItemService(ISpawnHandler spawnHandler)
        {
            _spawnHandler = spawnHandler;
        }
        public Item GenerateItemFromNoise(float noiseResult, int x, int y)
        {
            var item = RandomItemGenerator.GetRandomItem(noiseResult);
            if (item != null)
            {
                item.ItemId = (x + "!" + y + "!");
                _spawnHandler.SendSpawn(x, y, item);
            }
            return item;
        }

        public ISpawnHandler GetSpawnHandler()
        {
            return _spawnHandler;
        }
    }
}