using System;
using System.Collections.Generic;
using System.Linq;
using DataTransfer.DTO.Character;

namespace WorldGeneration
{
    public class World
    {
        private Map _map;
        private MapCharacterDTO CurrentPlayer { get; set; }
        private IList<MapCharacterDTO> Characters { get; set; }

        public World(IList<MapCharacterDTO> characters, MapCharacterDTO currentPlayer, int seed)
        {
            Characters = characters;
            _map = MapFactory.GenerateMap(seed: seed);
            CurrentPlayer = currentPlayer;
        }

        public void UpdateCharacterPosition(MapCharacterDTO characterPositionDTO)
        {
            if (CurrentPlayer.Name == characterPositionDTO.Name)
            {
                CurrentPlayer.XPosition = characterPositionDTO.XPosition;
                CurrentPlayer.YPosition = characterPositionDTO.YPosition;
            }
            
            var charactersWithTheSameName = Characters.Where(character => characterPositionDTO.Name == character.Name);
            if (charactersWithTheSameName.Count() > 1)
            {
                throw new Exception("Duplicate characters found in world");
            }
            if (charactersWithTheSameName.Count() > 0)
            {
                var character = charactersWithTheSameName.First();
                character.XPosition = characterPositionDTO.XPosition;
                character.YPosition = characterPositionDTO.YPosition;
            } else
            {
                Characters.Add(characterPositionDTO);
            }
        }

        public void DisplayWorld(int viewDistance)
        {
            // TOTO: Add characters 
            _map.DeleteMap();
            _map.DisplayMap(CurrentPlayer, viewDistance, Characters);
        }
    }
}
     
