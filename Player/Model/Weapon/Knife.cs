using Weapon.Enum;

namespace Weapon
{
    public class Knife : Weapon
    {
        
        public Knife()
        {
            Name = "Knife";
            WeaponType = WeaponType.Melee;
            Rarity = Rarity.Common;
            WeaponSpeed = WeaponSpeed.Slow;
            Distance = 1;
            Damage = 20;
        }
    }
}