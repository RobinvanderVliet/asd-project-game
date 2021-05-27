using Items.Consumable.ConsumableStats;

namespace Items.Consumable
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