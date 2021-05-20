using Player.Model.ItemStats;

namespace Player.Model.Weapon
{
    public class BaseBallBat : global::Weapon.Weapon
    {
        public BaseBallBat()
        {
            Name = "Baseball Bat";
            Type = WeaponType.Melee;
            Rarity = Rarity.Uncommon;
            Speed = WeaponSpeed.Slow;
            Distance = WeaponDistance.Close;
            Damage = WeaponDamage.High;
        }
    }
}