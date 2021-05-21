using Player.Model.Consumable.ConsumableStats;
using Player.Model.ItemStats;

namespace Player.Model.Consumable.StaminaConsumable
{
    public class BigMac : StaminaConsumable
    {
        public BigMac()
        {
            ItemName = "Big Mac";
            Stamina = Stamina.Low;
            Rarity = Rarity.Common;
        }
    }
}