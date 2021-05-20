using Weapon.Enum;

namespace Player.Model.Consumable.StaminaConsumable
{
    public class BigMac : StaminaConsumable
    {
        public BigMac()
        {
            Name = "Bandage";
            Stamina = 25;
            Rarity = Rarity.Common;
        }
    }
}