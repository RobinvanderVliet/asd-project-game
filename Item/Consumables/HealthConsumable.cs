using Items.Consumable.ConsumableStats;

namespace Items.Consumables
{
    public class HealthConsumable : Consumable
    {
        public Health Health { get; set; }

        public int getHealth()
        {
            return (int) Health;
        }
    }
}