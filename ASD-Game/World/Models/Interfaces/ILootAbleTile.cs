namespace ASD_project.World.Models.Interfaces
{
    public interface ILootAbleTile : ITile
    {
        int GenerateLoot();
        void LootItem(int item);
    }
}