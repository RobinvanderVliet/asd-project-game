using Weapon.Enum;

namespace Weapon
{
    public class Weapon
    {
        public WeaponSpeed WeaponSpeed { get; set; }
        public int Damage { get; set; }
        public int Distance { get; set; }

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