namespace Player.Model
{
    public class Item
    {
        public string ItemName { get; set; }
        public string Description { get; set; }

        public Item(string itemName, string decription)
        {
            ItemName = itemName;
            Description = decription;
        }
    }
}
