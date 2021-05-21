using Player.Model.Consumable.ConsumableStats;
using Player.Model.ItemStats;

namespace Player.Model.Consumable.StaminaConsumable
{
    public class SuspiciousWhitePowder : StaminaConsumable
    {
        private const string ConsumableDescription = "pink fluffy unicorns dancing on rainbows";
        public SuspiciousWhitePowder()
        {
            ItemName = "Suspicious white powder";
            Description = ConsumableDescription;
            Stamina = Stamina.High;
            Rarity = Rarity.Rare;
        }
    }
}