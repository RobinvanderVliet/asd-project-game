using ASD_project.Items.Consumables.ConsumableStats;

namespace ASD_project.Items.Consumables
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