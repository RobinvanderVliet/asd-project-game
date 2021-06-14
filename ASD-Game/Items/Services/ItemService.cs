using ASD_Game.ActionHandling;
using ASD_Game.World;

namespace ASD_Game.Items.Services
{
    public class ItemService : IItemService
    {
        private ISpawnHandler _spawnHandler;
        private IRandomItemGenerator _randomItemGenerator;
        public int ChanceThereIsAItem { get; set; }
        
        public ItemService(ISpawnHandler spawnHandler, IRandomItemGenerator randomItemGenerator, int chanceThereIsAItem = 60)
        {
            _spawnHandler = spawnHandler;
            _randomItemGenerator = randomItemGenerator;
            ChanceThereIsAItem = chanceThereIsAItem;
        }
        
        public Item GenerateItemFromNoise(float noiseResult, int x, int y)
        {
            var item = _randomItemGenerator.GetRandomItem(noiseResult, ChanceThereIsAItem);
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