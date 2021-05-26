using System;
using System.Collections.Generic;
using System.Linq;
using DataTransfer.DTO.Character;

namespace WorldGeneration
{
    public class World
    {
        public Map _map;
        public MapCharacterDTO CurrentPlayer { get; set; }
        private IList<MapCharacterDTO> _characters { get; set; }
        private readonly int _viewDistance;

        public World(int seed, int viewDistance)
        {
            _characters = new List<MapCharacterDTO>();
            // AddCharacterToWorld(currentPlayer);
            _map = MapFactory.GenerateMap(seed: seed);
            _viewDistance = viewDistance;
            _map.DeleteMap();
        }

        public void UpdateCharacterPosition(MapCharacterDTO characterPositionDTO)
        {
            var changed = false;

            if (CurrentPlayer.PlayerGuid == characterPositionDTO.PlayerGuid)
            {
                if (CurrentPlayer.XPosition != characterPositionDTO.XPosition ||
                    CurrentPlayer.YPosition != characterPositionDTO.YPosition)
                {
                    CurrentPlayer.XPosition = characterPositionDTO.XPosition;
                    CurrentPlayer.YPosition = characterPositionDTO.YPosition;
                    changed = true;
                }
            }

            if (_characters.Any(x => x.PlayerGuid.Equals(characterPositionDTO.PlayerGuid)))
            {
                var dto = _characters.
                    Where(x => x.PlayerGuid.Equals(characterPositionDTO.PlayerGuid)).FirstOrDefault();

                if (dto.XPosition != characterPositionDTO.XPosition ||
                    dto.YPosition != characterPositionDTO.YPosition)
                {
                    dto.XPosition = characterPositionDTO.XPosition;
                    dto.YPosition = characterPositionDTO.YPosition;
                    changed = true;
                }             
            }

            //Only rerender if the position actually changed.
            if (changed)
            {
                DisplayWorld();
            }
        }

        public void AddCharacterToWorld(MapCharacterDTO mapCharacterDTO, bool isCurrentPlayer)
        {
            if (isCurrentPlayer)
            {
                CurrentPlayer = mapCharacterDTO;
            }
            
            _characters.Add(mapCharacterDTO);
        }

        public void DisplayWorld()
        {
            if (CurrentPlayer is null || _characters is null)
            {
                return;
            }
            Console.Clear();
            _map.DisplayMap(CurrentPlayer, _viewDistance, _characters);
        }

        public void deleteMap()
        {
            _map.DeleteMap();
        }
    }
}
     
