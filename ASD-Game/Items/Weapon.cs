using System;
using ASD_Game.Items.ItemStats;
using ASD_Game.Items.WeaponStats;

namespace ASD_Game.Items
{
    public class Weapon : Item
    {
        public WeaponType Type { get; set; }
        public Rarity Rarity { get; set; }
        public WeaponSpeed Speed { get; set; }
        public WeaponDamage Damage { get; set; }
        public WeaponDistance Distance { get; set; }

        public override string ToString()
        {
            string inspect = Description;
            inspect += $"{Environment.NewLine}Type: {Type.ToString()}";
            inspect += $"{Environment.NewLine}Rarity: {Rarity.ToString()}";
            inspect += $"{Environment.NewLine}Damage: {Damage.ToString()}";
            inspect += $"{Environment.NewLine}Attack speed: {Speed.ToString()}";
            return inspect;
        }
    }
}