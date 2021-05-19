using Weapon.Enum;

namespace Weapon
{
    public class AK_47 : Weapon
    {
        private string _name;
        private WeaponType _weaponType;
        private Rarity _rarity;

        public AK_47()
        {
            _name = "AK_47";
            _weaponType = WeaponType.Range;
            _rarity = Rarity.Rare;
            WeaponSpeed = WeaponSpeed.Average;
            Distance = 5;
            Damage = 20;
        }
    }
}