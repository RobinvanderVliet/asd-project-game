using Weapon.Enum;

namespace Weapon
{
    public class Glock : Weapon
    {
        private string _name;
        private WeaponType _weaponType;
        private Rarity _rarity;
        
        public Glock()
        {
            _name = "Glock";
            _weaponType = WeaponType.Range;
            _rarity = Rarity.Uncommon;
            WeaponSpeed = WeaponSpeed.Slow;
            Distance = 3;
            Damage = 20;
        }
    }
}