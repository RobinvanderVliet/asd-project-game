using System.Collections.Generic;

namespace WorldGeneration
{
    public interface IMap
    {
        char[,] GetMapAroundCharacter(Player currentPlayer, int viewDistance, List<Character> characters);
        void DeleteMap();
    }
}