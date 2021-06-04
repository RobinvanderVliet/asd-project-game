using ASD_project.Items.Consumables.ConsumableStats;

namespace ASD_project.Items.Consumables
{
    public class StaminaConsumable : Consumable
    {
        public Stamina Stamina { get; set; }

        public int getStamina()
        {
            return (int) Stamina;
        }
    }
}