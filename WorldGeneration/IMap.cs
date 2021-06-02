using System.Collections.Generic;

namespace WorldGeneration
{
    public interface IMap
    {
        void DisplayMap(Player currentPlayer, int viewDistance, List<Character> characters);
        char[,] GetMapAroundCharacter(Player currentPlayer, int viewDistance, List<Character> characters);
        void DeleteMap();
        
    }
}