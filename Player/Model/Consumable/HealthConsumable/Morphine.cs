using Player.Model.Consumable.ConsumableStats;
using Weapon.Enum;

namespace Player.Model.Consumable.HealthConsumable
{
    public class Morphine : HealthConsumable
    {
        public Morphine()
        {
            Name = "Morphine";
            Health = Health.Medium;
            Rarity = Rarity.Uncommon;
        }
    }
}