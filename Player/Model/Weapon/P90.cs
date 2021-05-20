using Player.Model.ItemStats;

namespace Weapon
{
    public class P90 : Weapon
    {
        public P90()
        {
            Name = "P90";
            Type = WeaponType.Range;
            Rarity = Rarity.Rare;
            Speed = WeaponSpeed.Fast;
            Distance = WeaponDistance.Far;
            Damage = WeaponDamage.Low;
        }
    }
}