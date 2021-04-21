/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 3.
     
    Goal of this file: Generates tilemap at random.
     
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.Tiles;
using WorldGeneration.Tiles.Interfaces;

namespace WorldGeneration
{
    class TileMap
    {
        public IList<ITile> GenerateBaseTerrain()
        {
            IList<ITile> Map = new List<ITile>();
            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    var rnd = new Random().Next(0, 10);
                    switch (rnd)
                    {
                        case 0:
                            Map.Add(new StreetTile());
                            break;
                        case 1:
                            Map.Add(new GrassTile());
                            break;
                        case 2:
                            Map.Add(new DirtTile());
                            break;
                        case 3:
                            Map.Add(new HouseTile());
                            break;
                        case 4:
                            Map.Add(new GasTile(1));
                            break;
                        case 5:
                            Map.Add(new SpikeTile());
                            break;
                        case 6:
                            Map.Add(new ChestTile());
                            break;
                        case 7:
                            Map.Add(new DoorTile());
                            break;
                        case 8:
                            Map.Add(new WallTile());
                            break;
                        case 9:
                            Map.Add(new WaterTile());
                            break;
                        default:
                            Map.Add(new GrassTile());
                            break;
                    }                   
                }
            }
            return Map;
        }
    }
}
