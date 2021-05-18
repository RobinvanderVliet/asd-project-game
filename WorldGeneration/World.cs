using System;
using System.Collections.Generic;
using System.Linq;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
    public class World
    {
        private Map _map;
        public IList<IPlayer> _players { get; set; }
        public IList<ICharacter> _characters { get; set; }

        public World(IList<IPlayer> players, int seed)
        {
            _players = players;
            _map = MapFactory.GenerateMap(seed: seed);
        }

       
        public void DisplayWorld(int viewDistance, IPlayer player)
        {
            // in toekomst moet er ook characters worden meegegeven. 
            _map.DeleteMap();
            _map.DisplayMap(player, viewDistance, _players);
        }
    }
}
     
