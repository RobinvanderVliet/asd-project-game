namespace Player.Model
{
    public interface IPlayerModel
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Stamina { get; set; }

        //public Tile _currentTile { get; set; }
        public IInventory Inventory { get; set; }
        public IBitcoin Bitcoins { get; set; }
        public IRadiationLevel RadiationLevel { get; set; }

        public void AddHealth(int amount);

        public void RemoveHealth(int amount);

        public void AddStamina(int amount);

        public void RemoveStamina(int amount);

        public Item GetItem(string itemName);

        public void AddInventoryItem(Item item);

        public void RemoveInventoryItem(Item item);

        public void EmptyInventory();

        public void AddBitcoins(int amount);

        public void RemoveBitcoins(int amount);

        public int GetAttackDamage();

        public void PickupItem();

        public void DropItem(string itemName);

        public void HandleDirection(string direction, int steps);
        
        public int[] SendNewPosition(int[] newMovement);

    }
}