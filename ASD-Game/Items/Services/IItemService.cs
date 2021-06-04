using ActionHandling;
using ASD_project.World.Models.Interfaces;
using Items;

namespace ASD_project.World.Services
{
    public interface IItemService
    {
        public Item GenerateItemFromNoise(float noiseResult, int x, int y);
        public ISpawnHandler GetSpawnHandler();
    }
}