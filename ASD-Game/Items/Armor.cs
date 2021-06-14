using System;
using ASD_Game.Items.ArmorStats;
using ASD_Game.Items.ItemStats;

namespace ASD_Game.Items
{
    public class Armor : Item
    {
        public ArmorPartType ArmorPartType { get; set; }
        public int ArmorProtectionPoints { get; set; }
        public Rarity Rarity { get; set; }
        
        public override string ToString()
        {
            string inspect = Description;
            inspect += $"{Environment.NewLine}Name: {ItemName}";
            inspect += $"{Environment.NewLine}APP gain: {ArmorProtectionPoints}";
            inspect += $"{Environment.NewLine}Rarity: {Rarity.ToString()}";
            if (this is HazardProtectedArmor armor)
            {
                inspect += $"{Environment.NewLine}RPP gain: {armor.RadiationProtectionPoints}";
                inspect += $"{Environment.NewLine}Stamina points lost: {armor.StaminaPoints}";
            }
            return inspect;
        }
    }
}