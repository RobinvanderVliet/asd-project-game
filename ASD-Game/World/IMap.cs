using System.Collections.Generic;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World
{
    public interface IMap
    {
        public char[,] GetCharArrayMapAroundCharacter(Character currentPlayer, int viewDistance, List<Character> characters);
        public void DeleteMap();
        public ITile GetLoadedTileByXAndY(int x, int y);
        public void LoadArea(int playerX, int playerY, int viewDistance);

    }
}