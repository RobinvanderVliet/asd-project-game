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
        private readonly int _viewDistance;

        public World(int seed, int viewDistance, MapCharacterDTO currentPlayer)
        {
            Characters = new List<MapCharacterDTO>();
            Characters.Add(currentPlayer);
            _map = MapFactory.GenerateMap(seed: seed);
            CurrentPlayer = currentPlayer;
            _viewDistance = viewDistance;
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

            DisplayWorld();
        }

        public void DisplayWorld()
        {
            if (CurrentPlayer is null || Characters is null)
            {
                return;
            }
            // TODO: Add characters 
            Console.Clear();
            _map.DeleteMap(); // TODO: delete line
            _map.DisplayMap(CurrentPlayer, _viewDistance, Characters);
        }
    }
}
     
