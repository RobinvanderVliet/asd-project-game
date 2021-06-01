namespace WorldGeneration.Models.Interfaces
{
    public interface ILootAbleTile : ITile
    {
        int GenerateLoot();
        void LootItem(int item);
    }
}