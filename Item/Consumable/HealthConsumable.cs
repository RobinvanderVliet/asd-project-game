using Player.Model.Consumable.ConsumableStats;

namespace Player.Model.Consumable.HealthConsumable
{
    public class HealthConsumable : global::Item.Consumable.Consumable
    {
        public Health Health { get; set; }

        public int getHealth()
        {
            return (int) Health;
        }

    }
}