using ActionHandling.DTO;
using ASD_project.World.Models.Characters;

namespace ASD_project.World
{
    public interface IWorld
    {
        void UpdateCharacterPosition(string id, int newXPosition, int newYPosition);
        void AddPlayerToWorld(Player player, bool isCurrentPlayer = false);
        void AddCreatureToWorld(Models.Characters.Creature player);
        void UpdateMap();
        char[,] GetMapAroundCharacter(Character character);
        void DeleteMap();
        void AddItemToWorld(ItemSpawnDTO itemSpawnDto);
    }
}
