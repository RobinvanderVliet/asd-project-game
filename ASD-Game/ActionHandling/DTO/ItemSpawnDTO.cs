using ASD_project.Items;

namespace ASD_project.ActionHandling.DTO
{
    public class ItemSpawnDTO
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public Item Item { get; set; }
    }
}