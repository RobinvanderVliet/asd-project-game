using Weapon.Enum;

namespace Weapon
{
    public class Weapon
    {
        protected string Name { get; set; }
        protected WeaponType WeaponType { get; set; }
        protected Rarity Rarity { get; set; }
        protected WeaponSpeed WeaponSpeed { get; set; }
        protected int Damage { get; set; }
        protected int Distance { get; set; }

        public int GetWeaponSpeed()
        {
            return WeaponSpeed switch
            {
                WeaponSpeed.Fast => 2,
                WeaponSpeed.Average => 3,
                _ => 4
            };
        }
    }
}