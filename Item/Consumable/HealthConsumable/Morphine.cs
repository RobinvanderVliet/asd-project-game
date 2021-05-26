using Player.Model.Consumable.ConsumableStats;
using Player.Model.ItemStats;

namespace Player.Model.Consumable.HealthConsumable
{
    public class Morphine : HealthConsumable
    {
        private const string ConsumableDescription = "Comfortably numb!";


        public Morphine()
        {
            ItemName = "Morphine";
            Description = ConsumableDescription;
            Health = Health.Medium;
            Rarity = Rarity.Uncommon;
        }
    }
}