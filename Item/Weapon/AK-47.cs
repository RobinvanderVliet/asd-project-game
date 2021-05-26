using Player.Model.ItemStats;
using Player.Model.Weapon.WeaponStats;

namespace Weapon
{
    public class AK_47 : Weapon
    {
        private const string WeaponDescription = "She was a veiled threat";
        public AK_47()
        {
            ItemName = "AK-47";
            Description = WeaponDescription;
            Type = WeaponType.Range;
            Rarity = Rarity.Rare;
            Speed = WeaponSpeed.Average;
            Distance = WeaponDistance.Far;
            Damage = WeaponDamage.Low;
        }
    }
}