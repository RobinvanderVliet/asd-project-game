using Player.Model.Consumable.ConsumableStats;
using Player.Model.ItemStats;

namespace Player.Model.Consumable.HealthConsumable
{
    public class MedKit : HealthConsumable
    {
        public MedKit()
        {
            ItemName = "Medkit";
            Rarity = Rarity.Rare;
            Health = Health.High;
        }
    }
}