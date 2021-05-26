using Player.Model.Consumable.ConsumableStats;

namespace Player.Model.Consumable.StaminaConsumable
{
    public class StaminaConsumable : Consumable
    {
        protected Stamina Stamina { get; set; }

        public int getStamina()
        {
            return (int) Stamina;
        }
    }
}