using Player.Model.ItemStats;
using Player.Model.Weapon.WeaponStats;

namespace Player.Model.Weapon
{
    public class Katana : Weapon
    {
        private const string WeaponDescription = "Cutting edge technology.";
        public Katana()
        {
            ItemName = "Katana";
            Description = WeaponDescription;
            Type = WeaponType.Melee;
            Rarity = Rarity.Rare;
            Speed = WeaponSpeed.Fast;
            Distance = WeaponDistance.Close;
            Damage = WeaponDamage.Medium;
        }
    }
}