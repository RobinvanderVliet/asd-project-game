using ActionHandling;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
    public class ItemSpawner
    {
        private ISpawnHandler _spawnHandler;
        public ItemSpawner(ISpawnHandler spawnHandler)
        {
            _spawnHandler = spawnHandler;
        }

        
    }
}