using Player.Model.ItemStats;
using Player.Model.Weapon.WeaponStats;

namespace Weapon
{
    public class P90 : Weapon
    {
        private const string WeaponDescription = "CS:GO players hate him";
        public P90()
        {
            ItemName = "P90";
            Description = WeaponDescription;
            Type = WeaponType.Range;
            Rarity = Rarity.Rare;
            Speed = WeaponSpeed.Fast;
            Distance = WeaponDistance.Far;
            Damage = WeaponDamage.Low;
        }
    }
}