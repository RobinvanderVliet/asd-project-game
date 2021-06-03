using System.Collections.Generic;

namespace WorldGeneration
{
    public interface IMap
    {
        char[,] GetMapAroundCharacter(Character currentPlayer, int viewDistance, List<Character> characters);
        void DeleteMap();
    }
}