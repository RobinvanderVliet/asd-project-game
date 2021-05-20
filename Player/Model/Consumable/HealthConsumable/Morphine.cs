using Weapon.Enum;

namespace Player.Model.Consumable.HealthConsumable
{
    public class Morphine : HealthConsumable
    {
        public Morphine()
        {
            Name = "Morphine";
            Health = 50;
            Rarity = Rarity.Uncommon;
        }
    }
}