using ASD_project.Items.ItemStats;

namespace ASD_project.Items
{
    public interface IBuilder
    {
        public void Reset();
        public void SetName(string name);
        public void SetDescription(string description);
        public void SetRarity(Rarity rarity);
    }
}