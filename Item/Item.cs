namespace Items
{
    public class Item
    {
        private string _itemName;
        public string ItemName { get => _itemName; set => _itemName = value; }
        private string _description;
        public string Description { get => _description; set => _description = value; }
        
        public Item()
        {
        }
    }
}
