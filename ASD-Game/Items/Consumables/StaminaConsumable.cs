using ASD_Game.Items.Consumables.ConsumableStats;

namespace ASD_Game.Items.Consumables
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