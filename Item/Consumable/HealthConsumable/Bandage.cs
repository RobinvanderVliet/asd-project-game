using Player.Model.Consumable.ConsumableStats;
using Player.Model.ItemStats;

namespace Player.Model.Consumable.HealthConsumable
{
    public class Bandage : HealthConsumable
    {
        private const string ConsumableDescription = "Let me patch you together";
        public Bandage()
        {
            ItemName = "Bandage";
            Description = ConsumableDescription;
            Health = Health.Low;
            Rarity = Rarity.Common;
        }
    }
}