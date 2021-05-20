using Player.Model.ItemStats;

namespace Weapon
{
    public class Weapon
    {
        protected string Name { get; set; }
        protected WeaponType Type { get; set; }
        protected Rarity Rarity { get; set; }
        protected WeaponSpeed Speed { get; set; }
        protected WeaponDamage Damage { get; set; }
        protected WeaponDistance Distance { get; set; }

        public int GetWeaponSpeed()
        {
            return (int) Speed;
        }

        public int GetWeaponDamage()
        {
            return (int) Damage;
        }
        
        public int GetWeaponDistance()
        {
            return (int) Distance;
        }
    }
}