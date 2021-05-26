using Player.Model.ItemStats;
using Player.Model.Weapon.WeaponStats;

namespace Weapon
{
    public class Knife : Weapon
    {
        private const string WeaponDescription = "That ain't a knoife, this is a knoife";
        public Knife()
        {
            ItemName = "Knife";
            Description = WeaponDescription;
            Type = WeaponType.Melee;
            Rarity = Rarity.Common;
            Speed = WeaponSpeed.Slow;
            Distance = WeaponDistance.Close;
            Damage = WeaponDamage.Low;
        }
    }
}