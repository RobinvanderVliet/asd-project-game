using Weapon.Enum;

namespace Weapon
{
    public class Katana : Weapon
    {
        public Katana()
        {
            Name = "Katana";
            WeaponType = WeaponType.Melee;
            Rarity = Rarity.Rare;
            WeaponSpeed = WeaponSpeed.Fast;
            Distance = 1;
            Damage = 40;
        }
    }
}