using Items.ItemStats;
using Items.WeaponStats;

namespace Items
{
    public class Weapon : Item
    {
        public WeaponType Type { get; set; }
        public Rarity Rarity { get; set; }
        public WeaponSpeed Speed { get; set; }
        public WeaponDamage Damage { get; set; }
        public WeaponDistance Distance { get; set; }

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