using ASD_Game.ActionHandling.DTO;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;
using System.Collections.Generic;

namespace ASD_Game.World
{
    public interface IWorld
    {
        List<Player> Players { get; set; }
        Player CurrentPlayer { get; set; }
        List<Monster> Creatures { get; set; }
        List<ItemSpawnDTO> Items { get; set; }
        List<Character> MovesList { get; set; }
        public List<Character> GetAllCharacters();


        void UpdateCharacterPosition(string id, int newXPosition, int newYPosition);

        void AddPlayerToWorld(Player player, bool isCurrentPlayer = false);

        void AddCreatureToWorld(Monster monster);

        void UpdateMap();

        char[,] GetMapAroundCharacter(Character character);

        void DeleteMap();
        void AddItemToWorld(ItemSpawnDTO itemSpawnDto);
        void UpdateAI();
        void LoadArea(int playerX, int playerY, int viewDistance);
        Player GetPlayer(string id);
        Character GetAI(string id);
        ITile GetLoadedTileByXAndY(int x, int y);
        bool CheckIfCharacterOnTile(ITile tile);
        ITile GetCurrentTile();
        ITile GetTileForPlayer(Player player);
    }
}