using System;
using System.Collections.Generic;
using DataTransfer.DTO.Character;

namespace WorldGeneration
{
    public class World
    {
        private Map _map;
        private MapCharacterDTO CurrentPlayer { get; set; }
        private IList<MapCharacterDTO> Characters { get; set; }
        private readonly int _viewDistance;

        public World(int seed, int viewDistance)
        {
            Characters = new List<MapCharacterDTO>();
            // AddCharacterToWorld(currentPlayer);
            _map = MapFactory.GenerateMap(seed: seed);
            _viewDistance = viewDistance;
        }

        public void UpdateCharacterPosition(MapCharacterDTO characterPositionDTO)
        {
            // if (CurrentPlayer.Name == characterPositionDTO.Name)
            // {
            //     CurrentPlayer.XPosition = characterPositionDTO.XPosition;
            //     CurrentPlayer.YPosition = characterPositionDTO.YPosition;
            // }
            //
            // var charactersWithTheSameName = Characters.Where(character => characterPositionDTO.Name == character.Name);
            // if (charactersWithTheSameName.Count() > 1)
            // {
            //     throw new Exception("Duplicate characters found in world");
            // }
            // if (charactersWithTheSameName.Count() > 0)
            // {
            //     var character = charactersWithTheSameName.First();
            //     character.XPosition = characterPositionDTO.XPosition;
            //     character.YPosition = characterPositionDTO.YPosition;
            // } else
            // {
            //     throw new Exception("Could not find referenced character, it has not been initialized in the world");
            // }
            
            CurrentPlayer.XPosition = characterPositionDTO.XPosition;
            CurrentPlayer.YPosition = characterPositionDTO.YPosition;
            
            DisplayWorld();
        }

        public void AddCharacterToWorld(MapCharacterDTO mapCharacterDto, Boolean isCurrentPlayer)
        {
            if (isCurrentPlayer)
            {
                CurrentPlayer = mapCharacterDto;
            }
            
            Characters.Add(mapCharacterDto);
        }

        public void DisplayWorld()
        {
            if (CurrentPlayer is null || Characters is null)
            {
                return;
            }
            Console.Clear();
            _map.DisplayMap(CurrentPlayer, _viewDistance, Characters);
        }

        public void deleteMap()
        {
            _map.DeleteMap();
        }
    }
}
     
