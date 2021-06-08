using System.Collections.Generic;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
    public interface IMap
    {
        void DisplayMap(Character currentPlayer, int viewDistance, List<Character> characters);

        char[,] GetMapAroundCharacter(Character currentPlayer, int viewDistance, List<Character> characters);

        void DeleteMap();

        ITile GetLoadedTileByXAndY(int x, int y);

        void LoadArea(int playerX, int playerY, int viewDistance);
    }
}