using Player.Model.Consumable.ConsumableStats;
using Player.Model.ItemStats;

namespace Player.Model.Consumable.StaminaConsumable
{
    public class BigMac : StaminaConsumable
    {
        private const string ConsumableDescription = "What type of computer does Ronald McDonald use?";
        public BigMac()
        {
            ItemName = "Big Mac";
            Description = ConsumableDescription;
            Stamina = Stamina.Low;
            Rarity = Rarity.Common;
        }
    }
}