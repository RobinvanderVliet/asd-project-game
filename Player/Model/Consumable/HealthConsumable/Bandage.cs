using Weapon.Enum;

namespace Player.Model.Consumable.HealthConsumable
{
    public class Bandage : HealthConsumable
    {
        public Bandage()
        {
            Name = "Bandage";
            Health = 25;
            Rarity = Rarity.Common;
        }
    }
}