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

        public World(int seed, int viewDistance)
        {
            Characters = new List<MapCharacterDTO>();
            // AddCharacterToWorld(currentPlayer);
            _map = MapFactory.GenerateMap(seed: seed);
            _viewDistance = viewDistance;
        }

        public void UpdateCharacterPosition(MapCharacterDTO characterPositionDTO)
        {
            if (CurrentPlayer.Id == characterPositionDTO.Id)
            {
                CurrentPlayer.XPosition = characterPositionDTO.XPosition;
                CurrentPlayer.YPosition = characterPositionDTO.YPosition;
            }

            if (Characters.Any(x => x.Id.Equals(characterPositionDTO.Id)))
            {
                Characters.Where(x => x.Id.Equals(characterPositionDTO.Id)).FirstOrDefault().XPosition = characterPositionDTO.XPosition;
                Characters.Where(x => x.Id.Equals(characterPositionDTO.Id)).FirstOrDefault().YPosition = characterPositionDTO.YPosition;                
            }

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
     
