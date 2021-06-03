using System.Collections.Generic;
using ASD_project.World.Models.Characters;

namespace ASD_project.World
{
    public interface IMap
    {
        char[,] GetMapAroundCharacter(Character currentPlayer, int viewDistance, List<Character> characters);
        void DeleteMap();
    }
}