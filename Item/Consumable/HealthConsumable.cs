using Player.Model.Consumable.ConsumableStats;

namespace Items.Consumable
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