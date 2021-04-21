namespace WorldGeneration.Models.Interfaces
{
    public interface ILootableTile
    {
        int GenerateLoot();
        void LootItem(int item);
    }
}
