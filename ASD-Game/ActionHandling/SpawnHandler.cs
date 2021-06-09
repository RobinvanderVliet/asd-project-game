using System.Collections.Generic;
using System.Linq;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Items;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using Newtonsoft.Json;

namespace ASD_Game.ActionHandling
{
    public class SpawnHandler : ISpawnHandler
    {
        private IClientController _clientController;
        private readonly IDatabaseService<WorldItemPOCO> _worldItemDatabaseService;

        public SpawnHandler(IClientController clientController, IDatabaseService<WorldItemPOCO> worldItemDatabaseService)
        {
            _clientController = clientController;
            _worldItemDatabaseService = worldItemDatabaseService;
        }

        public void SendSpawn(int x, int y, Item item)
        {
            bool handleInDatabase = _clientController.IsHost() || _clientController.IsBackupHost;

            if (handleInDatabase)

            {
                WorldItemPOCO worldItem = new() { ItemName = item.ItemName, GameGUID = _clientController.SessionId, XPosition = x, YPosition = y };
                _worldItemDatabaseService.CreateAsync(worldItem);
            }
        }
    }
}