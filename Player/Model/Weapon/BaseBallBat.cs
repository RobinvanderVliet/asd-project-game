using Weapon.Enum;

namespace Weapon
{
    public class BaseBallBat : Weapon
    {
        public BaseBallBat()
        {
            Name = "BaseBallBat";
            WeaponType = WeaponType.Melee;
            Rarity = Rarity.Uncommon;
            WeaponSpeed = WeaponSpeed.Slow;
            Distance = 1;
            Damage = 60;
        }
    }
}