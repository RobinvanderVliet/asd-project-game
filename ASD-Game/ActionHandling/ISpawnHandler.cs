using System.Collections.Generic;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items;

namespace ASD_Game.ActionHandling
{
    public interface ISpawnHandler
    {
        public void SendSpawn(int x, int y, Item item);
    }
}