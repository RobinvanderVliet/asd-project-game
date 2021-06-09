using System.Collections.Generic;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World
{
    public interface IMap
    {
        void DisplayMap(Character currentPlayer, int viewDistance, List<Character> characters);

        char[,] GetCharArrayMapAroundCharacter(Character currentPlayer, int viewDistance, List<Character> characters);

        void DeleteMap();

        ITile GetLoadedTileByXAndY(int x, int y);

        void LoadArea(int playerX, int playerY, int viewDistance);
    }
}