using Player.Model.ItemStats;
using Player.Model.Weapon.WeaponStats;

namespace Player.Model.Weapon
{
    public class BaseBallBat : global::Weapon.Weapon
    {
        private const string WeaponDescription = "HOMERUN";

        public BaseBallBat()
        {
            ItemName = "Baseball Bat";
            Description = WeaponDescription;
            Type = WeaponType.Melee;
            Rarity = Rarity.Uncommon;
            Speed = WeaponSpeed.Slow;
            Distance = WeaponDistance.Close;
            Damage = WeaponDamage.High;
        }
    }
}