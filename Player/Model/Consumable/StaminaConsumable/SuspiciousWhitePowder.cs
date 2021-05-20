using Weapon.Enum;

namespace Player.Model.Consumable.StaminaConsumable
{
    public class SuspiciousWhitePowder : StaminaConsumable
    {
        public SuspiciousWhitePowder()
        {
            Name = "MonsterEnergy";
            Stamina = 100;
            Rarity = Rarity.Rare;
        }
    }
}