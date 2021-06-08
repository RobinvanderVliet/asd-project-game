using ASD_Game.Items.ItemStats;

namespace ASD_Game.Items
{
    public interface IBuilder
    {
        public void Reset();
        public void SetName(string name);
        public void SetDescription(string description);
        public void SetRarity(Rarity rarity);
    }
}