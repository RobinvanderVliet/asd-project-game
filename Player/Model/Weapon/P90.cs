using Player.Model.ItemStats;
using Player.Model.Weapon.WeaponStats;

namespace Weapon
{
    public class P90 : Weapon
    {
        public P90()
        {
            ItemName = "P90";
            Type = WeaponType.Range;
            Rarity = Rarity.Rare;
            Speed = WeaponSpeed.Fast;
            Distance = WeaponDistance.Far;
            Damage = WeaponDamage.Low;
        }
    }
}