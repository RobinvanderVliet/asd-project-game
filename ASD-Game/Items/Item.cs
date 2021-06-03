namespace Items
{
    public abstract class Item
    {
        private string _itemId;
        public string ItemId { get => _itemId; set => _itemId = value; }
        
        private string _itemName;
        public string ItemName { get => _itemName; set => _itemName = value; }
        private string _description;
        public string Description { get => _description; set => _description = value; }
    }
}
