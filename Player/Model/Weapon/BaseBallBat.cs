using Weapon.Enum;

namespace Weapon
{
    public class BaseBallBat : Weapon
    {
        private string _name;
        private WeaponType _weaponType;
        private Rarity _rarity;

       public BaseBallBat()
        {
            _name = "BaseBallBat";
            _weaponType = WeaponType.Melee;
            _rarity = Rarity.Uncommon;
            WeaponSpeed = WeaponSpeed.Slow;
            Distance = 1;
            Damage = 60;
        }
    }
}