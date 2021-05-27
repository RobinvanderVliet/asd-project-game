using Player.Model.Consumable.ConsumableStats;

namespace Item.Consumable
{
    public class StaminaConsumable : global::Item.Consumable.Consumable
    {
        public Stamina Stamina { get; set; }

        public int getStamina()
        {
            return (int) Stamina;
        }
    }
}