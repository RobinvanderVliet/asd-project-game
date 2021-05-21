using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor
{
    public class TacticalVest : Armor
    {
        public TacticalVest()
        {
            ItemName = "Tactical vest";
            ArmorType = ArmorType.Body;
            Rarity = Rarity.Rare;
            ArmorProtectionPoints = 40;
        }
    }
}