using Player.Model.Consumable.ConsumableStats;

namespace Player.Model.Consumable.HealthConsumable
{
    public class HealthConsumable : Consumable
    {
        protected Health Health { get; set; }

        public int getHealth()
        {
            return (int) Health;
        }

    }
}