using Weapon.Enum;

namespace Weapon
{
    public class Knife : Weapon
    {
        private string _name;
        private WeaponType _weaponType;
        private Rarity _rarity;
        
        public Knife()
        {
            _name = "Knife";
            _weaponType = WeaponType.Melee;
            _rarity = Rarity.Common;
            WeaponSpeed = WeaponSpeed.Slow;
            Distance = 1;
            Damage = 20;
        }
    }
}