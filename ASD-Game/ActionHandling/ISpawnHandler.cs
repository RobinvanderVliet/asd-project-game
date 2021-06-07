using System.Collections.Generic;
using ASD_project.ActionHandling.DTO;
using ASD_project.Items;

namespace ASD_project.ActionHandling
{
    public interface ISpawnHandler
    {
        public void SendSpawn(int x, int y, Item item);
        
        public void SetItemSpawnDtOs(List<ItemSpawnDTO> itemSpawnDTOs);
    }
}