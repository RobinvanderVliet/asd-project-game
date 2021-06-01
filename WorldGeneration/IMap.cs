using System.Collections.Generic;

namespace WorldGeneration
{
    public interface IMap
    {
        void DisplayMap(Player currentPlayer, int viewDistance, List<Player> characters);
        char[,] GetMapAroundCharacter(Player currentPlayer, int viewDistance, List<Player> characters);
        void DeleteMap();
    }
}