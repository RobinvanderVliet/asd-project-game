namespace Player
{
    public interface IInventoryHandler
    {
        public void UseItem(int index);
        public void PickupItem(int index);
        public void DropItem(int index);
    }
}