namespace DataTransfer.Model.World.Interfaces
{
    public interface ILootAbleTile : ITile
    {
        int GenerateLoot();
        void LootItem(int item);
    }
}