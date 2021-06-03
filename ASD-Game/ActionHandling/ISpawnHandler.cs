using Items;

namespace ActionHandling
{
    public interface ISpawnHandler
    {
        public void SendSpawn(int x, int y, Item item);
    }
}