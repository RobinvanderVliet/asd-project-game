using System.Collections.Generic;
using ActionHandling.DTO;
using Items;

namespace ActionHandling
{
    public interface ISpawnHandler
    {
        public void SendSpawn(int x, int y, Item item);
        void setItemSpawnDTOs(List<ItemSpawnDTO> itemSpawnDTOs);
    }
}