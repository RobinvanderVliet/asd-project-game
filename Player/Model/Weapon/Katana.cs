using Weapon.Enum;

namespace Weapon
{
    public class Katana : Weapon
    {
        private string _name;
        private WeaponType _weaponType;
        private Rarity _rarity;
        
        public Katana()
        {
            _name = "Katana";
            _weaponType = WeaponType.Melee;
            _rarity = Rarity.Rare;
            WeaponSpeed = WeaponSpeed.Fast;
            Distance = 1;
            Damage = 40;
        }
    }
}