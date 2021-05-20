using Player.Model.ItemStats;

namespace Weapon
{
    public class Katana : Weapon
    {
        public Katana()
        {
            Name = "Katana";
            Type = WeaponType.Melee;
            Rarity = Rarity.Rare;
            Speed = WeaponSpeed.Fast;
            Distance = WeaponDistance.Close;
            Damage = WeaponDamage.Medium;
        }
    }
}