using Player.Model.Consumable.ConsumableStats;
using Player.Model.ItemStats;

namespace Player.Model.Consumable.StaminaConsumable
{
    public class SuspiciousWhitePowder : StaminaConsumable
    {
        public SuspiciousWhitePowder()
        {
            ItemName = "Suspicious white powder";
            Stamina = Stamina.High;
            Rarity = Rarity.Rare;
        }
    }
}