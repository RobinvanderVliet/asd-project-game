using System.Collections.Generic;
using ActionHandling.DTO;
using Items;

namespace ActionHandling
{
    public interface ISpawnHandler
    {
        public void SendSpawn(int x, int y, Item item);
        
        public void setItemSpawnDTOs(List<ItemSpawnDTO> itemSpawnDTOs);
    }
}