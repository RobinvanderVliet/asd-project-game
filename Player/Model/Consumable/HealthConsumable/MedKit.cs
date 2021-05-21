using Player.Model.Consumable.ConsumableStats;
using Player.Model.ItemStats;

namespace Player.Model.Consumable.HealthConsumable
{
    public class MedKit : HealthConsumable
    {
        private const string ConsumableDescription = "Good as new";
        public MedKit()
        {
            ItemName = "Medkit";
            Description = ConsumableDescription;
            Rarity = Rarity.Rare;
            Health = Health.High;
        }
    }
}