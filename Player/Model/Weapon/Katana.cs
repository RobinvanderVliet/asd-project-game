using Player.Model.ItemStats;
using Player.Model.Weapon.WeaponStats;

namespace Weapon
{
    public class Katana : Weapon
    {
        public Katana()
        {
            ItemName = "Katana";
            Type = WeaponType.Melee;
            Rarity = Rarity.Rare;
            Speed = WeaponSpeed.Fast;
            Distance = WeaponDistance.Close;
            Damage = WeaponDamage.Medium;
        }
    }
}