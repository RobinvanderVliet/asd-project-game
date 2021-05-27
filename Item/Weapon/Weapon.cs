using Item.Weapon.WeaponStats;
using Player.Model;
using Player.Model.ItemStats;

namespace Item.Weapon
{
    public class Weapon : IItem
    {
        public string Description { get; set; }
        public string ItemName { get; set; }
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