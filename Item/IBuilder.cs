using Player.Model.ItemStats;

namespace Item
{
    public interface IBuilder
    {
        public void Reset();
        public void SetName(string name);
        public void SetDescription(string description);
        public void SetRarity(Rarity rarity);
    }
}