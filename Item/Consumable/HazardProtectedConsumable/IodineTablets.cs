
using Player.Model.ItemStats;

namespace Player.Model.Consumable.HazardProtectedConsumable
{
    public class IodineTablets : HazardProtectedConsumable
    {
        private const string ConsumableDescription = "What do you call a child with Iodine deficiency? Chld";
        public IodineTablets()
        {
            ItemName = "Iodine tablets";
            Description = ConsumableDescription;
            RPP = 20;
            Rarity = Rarity.Common;
        }
    }
}