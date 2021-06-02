using Player.Model.ItemStats;
using Player.Model.Weapon.WeaponStats;

namespace Player.Model.Weapon
{
    public class Glock : Weapon
    {
        private const string WeaponDescription = "Ate a glock";
        public Glock()
        {
            ItemName = "Glock";
            Description = WeaponDescription;
            Type = WeaponType.Range;
            Rarity = Rarity.Uncommon;
            Speed = WeaponSpeed.Slow;
            Distance = WeaponDistance.Medium;
            Damage = WeaponDamage.Low;
        }
    }
}