/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 3.
     
    Goal of this file: Generates tilemap at random.
     
*/

using System;
using System.Collections.Generic;
using WorldGeneration.Tiles;

namespace WorldGeneration
{
    class TileMap
    {
        public IList<Tile> generateBaseTerrain()
        {
            IList<Tile> Map = new List<Tile>();
            for (var x = 0; x < 40; x++)
            {
                for (var y = 0; y < 80; y++)
                {
                    var rnd = new Random().Next(0, 10);
                    switch (rnd)
                    {
                        case 0:
                            Map.Add(new StreetTile(x, y));
                            break;
                        case 1:
                            Map.Add(new GrassTile(x, y));
                            break;
                        case 2:
                            Map.Add(new DirtTile(x, y));
                            break;
                        case 3:
                            Map.Add(new HouseTile(x, y));
                            break;
                        case 4:
                            Map.Add(new GasTile(x, y, 1));
                            break;
                        case 5:
                            Map.Add(new SpikeTile(x, y));
                            break;
                        case 6:
                            Map.Add(new ChestTile(x, y));
                            break;
                        case 7:
                            Map.Add(new DoorTile(x, y));
                            break;
                        case 8:
                            Map.Add(new WallTile(x, y));
                            break;
                        case 9:
                            Map.Add(new WaterTile(x, y));
                            break;
                        default:
                            Map.Add(new GrassTile(x, y));
                            break;
                    }
                }
            }
            return Map;
        }
    }
}
