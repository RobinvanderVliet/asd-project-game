namespace Items
{
    public abstract class Item
    {
        private string _itemName;
        public string ItemName { get => _itemName; set => _itemName = value; }
        private string _description;
        public string Description { get => _description; set => _description = value; }
        private int _rarity;
        public int Rarity { get => _rarity; set => _rarity = value; }
    }
}
