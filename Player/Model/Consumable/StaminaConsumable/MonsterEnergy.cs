using Weapon.Enum;

namespace Player.Model.Consumable.StaminaConsumable
{
    public class MonsterEnergy : StaminaConsumable
    {
        public MonsterEnergy()
        {
            Name = "MonsterEnergy";
            Stamina = 50;
            Rarity = Rarity.Uncommon;
        }
    }
}