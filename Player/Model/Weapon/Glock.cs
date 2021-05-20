using Weapon.Enum;

namespace Weapon
{
    public class Glock : Weapon
    {
        public Glock()
        {
            Name = "Glock";
            WeaponType = WeaponType.Range;
            Rarity = Rarity.Uncommon;
            WeaponSpeed = WeaponSpeed.Slow;
            Distance = 3;
            Damage = 20;
        }
    }
}