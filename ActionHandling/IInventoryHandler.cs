namespace ActionHandling
{
    public interface IInventoryHandler
    {
        public void Search();
        public void InspectItem(string slot);
    }
}