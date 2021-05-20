using Player.Model.Consumable.ConsumableStats;
using Weapon.Enum;


namespace Player.Model.Consumable.HealthConsumable
{
    public class Bandage : HealthConsumable
    {
        public Bandage()
        {
            Name = "Bandage";
            Health = Health.Low;
            Rarity = Rarity.Common;
        }
    }
}