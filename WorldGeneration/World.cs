using System;
using System.Collections.Generic;
using System.Linq;
using DataTransfer.DTO.Character;

namespace WorldGeneration
{
    public class World
    {
        private IMap _map;
        public MapCharacterDTO CurrentPlayer { get; set; }
        private IList<MapCharacterDTO> _characters { get; set; }
        private readonly int _viewDistance;

        public World(int seed, int viewDistance, IMapFactory mapFactory)
        {
            _characters = new List<MapCharacterDTO>();
            _map = mapFactory.GenerateMap(seed);
            _viewDistance = viewDistance;
            DeleteMap();
        }

        public void UpdateCharacterPosition(MapCharacterDTO characterPositionDTO)
        {
            if (CurrentPlayer.PlayerGuid == characterPositionDTO.PlayerGuid)
            {
                CurrentPlayer.XPosition = characterPositionDTO.XPosition;
                CurrentPlayer.YPosition = characterPositionDTO.YPosition;
            }

            if (_characters.Any(x => x.PlayerGuid.Equals(characterPositionDTO.PlayerGuid)))
            {
                _characters.Where(x => x.PlayerGuid.Equals(characterPositionDTO.PlayerGuid)).FirstOrDefault().XPosition = characterPositionDTO.XPosition;
                _characters.Where(x => x.PlayerGuid.Equals(characterPositionDTO.PlayerGuid)).FirstOrDefault().YPosition = characterPositionDTO.YPosition;                
            }

            DisplayWorld();
        }

        public void AddCharacterToWorld(MapCharacterDTO mapCharacterDto, bool isCurrentPlayer)
        {
            if (isCurrentPlayer)
            {
                CurrentPlayer = mapCharacterDto;
            }
            _characters.Add(mapCharacterDto);
        }

        public void DisplayWorld()
        {
            if (CurrentPlayer is null || _characters is null)
            {
                return;
            }
            _map.DisplayMap(CurrentPlayer, _viewDistance, _characters);
        }

        public void DeleteMap()
        {
            _map.DeleteMap();
        }
    }
}
     
