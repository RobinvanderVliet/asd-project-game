using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using DataTransfer.DTO.Character;
using DataTransfer.DTO.Player;
using Player.DTO;
using Player.Model;
using WorldGeneration.Models.Interfaces;

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
            var characters = Characters.Where(character => characterPositionDTO.Name == character.Name);
            if (characters.Count() > 1)
            {
                throw new Exception("Duplicate characters found in world");
            }
            if (characters.Count() > 0)
            {
                var character = characters.First();
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
     
