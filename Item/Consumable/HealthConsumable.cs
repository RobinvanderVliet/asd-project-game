using Player.Model.Consumable.ConsumableStats;

namespace Item.Consumable
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