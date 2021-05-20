using Weapon.Enum;

namespace Weapon
{
    public class AK_47 : Weapon
    {
        public AK_47()
        {
            Name = "AK_47";
            WeaponType = WeaponType.Range;
            Rarity = Rarity.Rare;
            WeaponSpeed = WeaponSpeed.Average;
            Distance = 5;
            Damage = 20;
        }
    }
}