using System.Collections.Generic;
using ASD_project.World.Models.Characters;

namespace ASD_project.World
{
    public interface IMap
    {
        char[,] GetCharArrayMapAroundCharacter(Character currentPlayer, int viewDistance, List<Character> characters);
        void DeleteMap();
        void DisplayMap(Character currentPlayer, int viewDistance, List<Character> characters);

    }
}