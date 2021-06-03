using System.Collections.Generic;

namespace WorldGeneration
{
    public interface IMap
    {
        void DisplayMap(Character currentPlayer, int viewDistance, List<Character> characters);

        char[,] GetMapAroundCharacter(Character currentPlayer, int viewDistance, List<Character> characters);

        void DeleteMap();
    }
}