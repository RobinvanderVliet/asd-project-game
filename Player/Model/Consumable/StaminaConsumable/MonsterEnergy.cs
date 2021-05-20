using Player.Model.Consumable.ConsumableStats;
using Weapon.Enum;

namespace Player.Model.Consumable.StaminaConsumable
{
    public class MonsterEnergy : StaminaConsumable
    {
        public MonsterEnergy()
        {
            Name = "Monster energy";
            Stamina = Stamina.Medium;
            Rarity = Rarity.Uncommon;
        }
    }
}