using Weapon.Enum;

namespace Player.Model.Consumable.HazardProtectedConsumable
{
    public class IodineTablets : HazardProtectedConsumable
    {
        public IodineTablets()
        {
            Name = "Iodine tablets";
            RPP = 20;
            Rarity = Rarity.Common;
        }
    }
}