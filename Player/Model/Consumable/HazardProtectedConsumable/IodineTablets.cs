using Weapon.Enum;

namespace Player.Model.Consumable.HazardProtectedConsumable
{
    public class IodineTablets : HazardProtectedConsumable
    {
        public IodineTablets()
        {
            Name = "MonsterEnergy";
            RPP = 20;
            Rarity = Rarity.Common;
        }
    }
}