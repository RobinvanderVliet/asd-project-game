using Items.Consumables.ConsumableStats;

namespace Items.Consumables
{
    public class StaminaConsumable : Consumable
    {
        public Stamina Stamina { get; set; }

        public int getStamina()
        {
            return (int)Stamina;
        }
    }
}