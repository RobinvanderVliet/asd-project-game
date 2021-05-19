using Weapon.Enum;

namespace Weapon
{
    public class P90 : Weapon
    {
        private string _name;
        private WeaponType _weaponType;
        private Rarity _rarity;
        
        public P90()
        {
            _name = "P90";
            _weaponType = WeaponType.Range;
            _rarity = Rarity.Rare;
            WeaponSpeed = WeaponSpeed.Fast;
            Distance = 5;
            Damage = 20;
        }
    }
}