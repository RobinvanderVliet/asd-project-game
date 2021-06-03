using System.Collections.Generic;
using ActionHandling.DTO;
using ASD_project.World.Models.Characters;

namespace ASD_project.World.Services
{
    public interface IWorldService
    {
        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition);
        public void AddPlayerToWorld(Player player, bool isCurrentPlayer);
        public void DisplayWorld();
        public void DeleteMap();
        public void GenerateWorld(int seed);
        public Player getCurrentPlayer();
        public List<ItemSpawnDTO> getAllItems();
        public void AddItemToWorld(ItemSpawnDTO itemSpawnDto);
    }
}