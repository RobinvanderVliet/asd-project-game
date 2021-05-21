using Player.Model.Consumable.ConsumableStats;
using Player.Model.ItemStats;

namespace Player.Model.Consumable.HealthConsumable
{
    public class Bandage : HealthConsumable
    {
        public Bandage()
        {
            ItemName = "Bandage";
            Health = Health.Low;
            Rarity = Rarity.Common;
        }
    }
}