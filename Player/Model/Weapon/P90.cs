using Weapon.Enum;

namespace Weapon
{
    public class P90 : Weapon
    {
        public P90()
        {
            Name = "P90";
            WeaponType = WeaponType.Range;
            Rarity = Rarity.Rare;
            WeaponSpeed = WeaponSpeed.Fast;
            Distance = 5;
            Damage = 20;
        }
    }
}