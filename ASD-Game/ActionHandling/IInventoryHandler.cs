namespace ASD_Game.ActionHandling
{
    public interface IInventoryHandler
    {
        public void UseItem(int index);
        public void PickupItem(int index);
        public void DropItem(string inventorySlot);
        public void Search();
        public void InspectItem(string slot);
    }
}