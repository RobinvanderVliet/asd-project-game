using ASD_project.Items.ArmorStats;
using ASD_project.Items.ItemStats;

namespace ASD_project.Items
{
    public class Armor : Item
    {
        public ArmorPartType ArmorPartType { get; set; }
        public int ArmorProtectionPoints { get; set; }
        public Rarity Rarity { get; set; }
    }
}