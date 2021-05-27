using Player.Model.ItemStats;

namespace Player.Model
{
    public interface IBuilder
    {
        public void Reset();
        public void SetName(string name);
        public void SetDescription(string description);
        public void SetRarity(Rarity rarity);
    }
}