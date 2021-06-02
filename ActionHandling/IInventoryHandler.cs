namespace ActionHandling
{
    public interface IInventoryHandler
    {
        public void UseItem(int index);
        public void PickupItem(int index);
        public void DropItem(int index);
        public void Search();
        public void InspectItem(string slot);
    }
}