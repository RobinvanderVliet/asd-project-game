using Player.Model.Consumable.ConsumableStats;
using Player.Model.ItemStats;

namespace Player.Model.Consumable.StaminaConsumable
{
    public class MonsterEnergy : StaminaConsumable
    {
        private const string ConsumableDescription = "WARNING: contains real monsters";
        public MonsterEnergy()
        {
            ItemName = "Monster energy";
            Description = ConsumableDescription;
            Stamina = Stamina.Medium;
            Rarity = Rarity.Uncommon;
        }
    }
}