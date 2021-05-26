using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor
{
    public class TacticalVest : Armor
    {
        private const string ArmorDescription = "Bullets got nothing on this!";

        public TacticalVest()
        {
            ItemName = "Tactical vest";
            Description = ArmorDescription;
            ArmorType = ArmorType.Body;
            Rarity = Rarity.Rare;
            ArmorProtectionPoints = 40;
        }
    }
}