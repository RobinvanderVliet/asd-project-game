using Player.Model.ItemStats;

namespace Weapon
{
    public class Knife : Weapon
    {
        
        public Knife()
        {
            Name = "Knife";
            Type = WeaponType.Melee;
            Rarity = Rarity.Common;
            Speed = WeaponSpeed.Slow;
            Distance = WeaponDistance.Close;
            Damage = WeaponDamage.Low;
        }
    }
}