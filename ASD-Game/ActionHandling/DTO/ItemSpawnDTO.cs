using Items;

namespace ActionHandling.DTO
{
    public class ItemSpawnDTO
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public Item item { get; set; }
    }
}