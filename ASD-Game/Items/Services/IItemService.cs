using ASD_project.ActionHandling;

namespace ASD_project.Items.Services
{
    public interface IItemService
    {
        public Item GenerateItemFromNoise(float noiseResult, int x, int y);
        public ISpawnHandler GetSpawnHandler();
    }
}