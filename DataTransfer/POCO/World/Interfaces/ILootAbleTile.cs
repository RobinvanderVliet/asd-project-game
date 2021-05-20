namespace DataTransfer.POCO.World.Interfaces
{
    public interface ILootAbleTile : ITile
    {
        int GenerateLoot();
        void LootItem(int item);
    }
}