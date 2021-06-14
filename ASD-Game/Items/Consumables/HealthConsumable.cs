using ASD_Game.Items.Consumables.ConsumableStats;

namespace ASD_Game.Items.Consumables
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