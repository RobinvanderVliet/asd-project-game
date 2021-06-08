using ASD_Game.ActionHandling.DTO;
using ASD_Game.World.Models.Characters;

namespace ASD_Game.World
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
