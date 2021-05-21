using Player.Model.ItemStats;
using Player.Model.Weapon.WeaponStats;

namespace Player.Model.Weapon
{
    public class BaseBallBat : global::Weapon.Weapon
    {
        public BaseBallBat()
        {
            ItemName = "Baseball Bat";
            Type = WeaponType.Melee;
            Rarity = Rarity.Uncommon;
            Speed = WeaponSpeed.Slow;
            Distance = WeaponDistance.Close;
            Damage = WeaponDamage.High;
        }
    }
}