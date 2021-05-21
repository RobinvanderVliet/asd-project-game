using Player.Model.ItemStats;
using Player.Model.Weapon.WeaponStats;

namespace Weapon
{
    public class Glock : Weapon
    {
        public Glock()
        {
            ItemName = "Glock";
            Type = WeaponType.Range;
            Rarity = Rarity.Uncommon;
            Speed = WeaponSpeed.Slow;
            Distance = WeaponDistance.Medium;
            Damage = WeaponDamage.Low;
        }
    }
}