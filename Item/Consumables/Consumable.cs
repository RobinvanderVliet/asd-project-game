using System;
using Items.ItemStats;

namespace Items.Consumables
{
    public class Consumable : Item
    {
        public Rarity Rarity { get; set; }

        public Consumable()
        {

        }
        
        
        public override string ToString()
        {
            string inspect = Description + $"{Environment.NewLine}Name: {ItemName}";
            inspect += $"{Environment.NewLine}Rarity: {Rarity.ToString()}";

            if (this is StaminaConsumable consumableStamina)
            {
                inspect += $"{Environment.NewLine}Stamina gain: {consumableStamina.Stamina.ToString()}";
            }
            else if (this is HealthConsumable consumableHealth)
            {
                inspect += $"{Environment.NewLine}Health gain: {consumableHealth.Health.ToString()}";
            }
            else if (this is HazardProtectedConsumable consumableHazzard)
            {
                inspect += $"{Environment.NewLine}RPP gain: {consumableHazzard.RPP.ToString()}";
            }
        
            return inspect;
        }
    }
}