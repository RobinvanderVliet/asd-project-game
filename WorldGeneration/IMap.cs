using System.Collections.Generic;
using DataTransfer.DTO.Character;
using DataTransfer.Model.World.Interfaces;

namespace WorldGeneration
{
    public interface IMap
    {
        void DisplayMap(MapCharacterDTO currentPlayer, int viewDistance, IList<MapCharacterDTO> characters);
        void DeleteMap();
    }
}