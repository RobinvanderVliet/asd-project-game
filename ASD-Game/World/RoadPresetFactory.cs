using System.Collections;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.World.Models.TerrainTiles;
using WorldGeneration.Models.Enums;

namespace WorldGeneration
{
    public class RoadPresetFactory
    {
        // Provides a set of preset road "templates" that connect the correct sides.
        private Hashtable _singletonRoadPresets;
        private int _chunksize;

        public RoadPresetFactory(int chunksize)
        {
            this._chunksize = chunksize;
            _singletonRoadPresets = GenerateRoadPresets();
        }

        public ITile[] GetRoadPreset (CompassDirections direction)
        {
            ITile[] result = _singletonRoadPresets[direction] as ITile[];
            return result;
        }

        private Hashtable GenerateRoadPresets()
        {
            var roadPresets = new Hashtable
            {
                {CompassDirections.NoRoads, NoRoads()},
                {CompassDirections.EastOnly, EastOnly()},
                {CompassDirections.WestOnly, WestOnly()},
                {CompassDirections.NorthOnly, NorthOnly()},
                {CompassDirections.SouthOnly, SouthOnly()},
                {CompassDirections.NorthToSouth, NorthToSouth()},
                {CompassDirections.NorthToEast, NorthToEast()},
                {CompassDirections.NorthToWest, NorthToWest()},
                {CompassDirections.EastToWest, EastToWest()},
                {CompassDirections.EastToSouth, EastToSouth()},
                {CompassDirections.WestToSouth, WestToSouth()},
                {CompassDirections.AllButNorth, AllButNorth()},
                {CompassDirections.AllButSouth, AllButSouth()},
                {CompassDirections.AllButEast, AllButEast()},
                {CompassDirections.AllButWest, AllButWest()},
                {CompassDirections.AllRoads, AllRoads()}
            };
            return roadPresets;
        }

        private ITile[] NoRoads()
        {
            // If there are no roads, return an empty roadmap.
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            return roadmap;
        }
        
        private ITile[] WestOnly ()
        {
            // First, if there is an even number of tiles in a chunk, we need two rows of roads. Otherwise, we'll only use one.
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            { 
                // Example using chunksize of 8:
                // half of chunksize = 4;
                // 3 * 8 = 24. Because we start at 0, this is the first tile of the fourth row.
                // Instead of filling the next two rows, only fill the first half of both rows. The easiest ways to do this is do two loops.
                int halfwayinthechunk = ((_chunksize /2)-1) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + (_chunksize/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = halfwayinthechunk + _chunksize; i < halfwayinthechunk + _chunksize + (_chunksize/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
            }
            else
            {
                // Simple example using chunksize of 9:
                // half of chunksize = 4;
                // 4 * 9 = 36. This is the first tile of the fifth row.
                // Instead of filling the entire row, just fill the first half +1.
                int halfwayinthechunk = ((_chunksize -1) / 2) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + ((_chunksize +1)/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
            }
            return roadmap;
        }
        
        private ITile[] EastOnly ()
                {
                    // First, if there is an even number of tiles in a chunk, we need two rows of roads. Otherwise, we'll only use one.
                    ITile[] roadmap = new ITile[_chunksize * _chunksize];
                    if (_chunksize % 2 == 0)
                    {
                        // Uses similar logic to EastOnly, but instead of filling the first half of the row, fill the second half.
                        int halfwayinthechunk = ((_chunksize /2)-1) * _chunksize;
                        for (int i = halfwayinthechunk + (_chunksize/2); i < halfwayinthechunk + _chunksize; i++)
                        {
                            roadmap[i] = new StreetTile(0, 0);
                        }
                        for (int i = halfwayinthechunk + _chunksize + (_chunksize/2); i < halfwayinthechunk + (_chunksize*2); i++)
                        {
                            roadmap[i] = new StreetTile(0, 0);
                        }
                    }
                    else
                    {
                        int halfwayinthechunk = ((_chunksize -1) / 2) * _chunksize;
                        for (int i = halfwayinthechunk + ((_chunksize -1)/2); i < halfwayinthechunk + _chunksize  ; i++)
                        {
                            roadmap[i] = new StreetTile(0, 0);
                        }
                    }
                    return roadmap;
                }
        
        private ITile[] NorthOnly ()
        {
            // First, if there is an even number of tiles in a chunk, we need two rows of roads. Otherwise, we'll only use one.
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                for (int i = (_chunksize/2)-1; i < (_chunksize* _chunksize)/2; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                for (int i = (_chunksize-1)/2; i < (_chunksize*_chunksize)/2; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            
            return roadmap;
        }
        
        private ITile[] SouthOnly ()
        {
            // First, if there is an even number of tiles in a chunk, we need two rows of roads. Otherwise, we'll only use one.
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                for (int i = (_chunksize * _chunksize/2) + (_chunksize/2)-1; i < ((_chunksize*_chunksize)-1); i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                for (int i = (_chunksize * _chunksize/2) + ((_chunksize-1)/2); i < (_chunksize*_chunksize)-1; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            
            return roadmap;
        }
        
        private ITile[] NorthToSouth ()
        {
            // First, if there is an even number of tiles in a chunk, we need two rows of roads. Otherwise, we'll only use one.
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                for (int i = (_chunksize/2)-1; i < (_chunksize* _chunksize)-1; i += _chunksize)
                {
                    // Simple example using chunksize of 8:
                    // Half of 8 is 4, so the loop begins at 3. Because the list begins at 0, we need to subtract one. Tile 3 and 4 (i+1) are turned into street tiles.
                    // Then, iterating by 8, the next tile is 11 and 12, then so on.
                    // Note that the x an y coordinates will be overwritten later, but they are mandatory.
                    
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                for (int i = (_chunksize-1)/2; i < (_chunksize*_chunksize)-1; i += _chunksize)
                {
                    // Simple example using chunksize of 9:
                    // Half of 8 is 4, so the loop begins at 4. (Because our list begins at 0, this is the fifth tile).
                    // Tile 4 is turned into a street tile. Then, iterating by 9, the next tile is 13, then so on.
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            
            return roadmap;
        }

        private ITile[] NorthToEast()
        {
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                int halfwayinthechunk = ((_chunksize /2)-1) * _chunksize;
                for (int i = halfwayinthechunk + (_chunksize/2); i < halfwayinthechunk + _chunksize; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = halfwayinthechunk + _chunksize + (_chunksize/2); i < halfwayinthechunk + (_chunksize*2); i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize/2)-1; i < (_chunksize* _chunksize)/2; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                int halfwayinthechunk = ((_chunksize -1) / 2) * _chunksize;
                for (int i = halfwayinthechunk + ((_chunksize -1)/2); i < halfwayinthechunk + _chunksize  ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize-1)/2; i < (_chunksize*_chunksize)/2; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            
            return roadmap;
        }
        
        private ITile[] NorthToWest()
        {
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                int halfwayinthechunk = ((_chunksize /2)-1) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + (_chunksize/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = halfwayinthechunk + _chunksize; i < halfwayinthechunk + _chunksize + (_chunksize/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize/2)-1; i < (_chunksize* _chunksize)/2; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                int halfwayinthechunk = ((_chunksize -1) / 2) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + ((_chunksize +1)/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize-1)/2; i < (_chunksize*_chunksize)/2; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            
            return roadmap;
        }
        
        private ITile[] EastToWest ()
        {
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                int halfwayinthechunk = ((_chunksize /2)-1) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + (_chunksize*2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
            }
            else
            {
                int halfwayinthechunk = ((_chunksize -1) / 2) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + _chunksize ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
            }
            
            return roadmap;
        }
        
        private ITile[] EastToSouth()
        {
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                int halfwayinthechunk = ((_chunksize /2)-1) * _chunksize;
                for (int i = halfwayinthechunk + (_chunksize/2); i < halfwayinthechunk + _chunksize; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = halfwayinthechunk + _chunksize + (_chunksize/2); i < halfwayinthechunk + (_chunksize*2); i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize * _chunksize/2) + (_chunksize/2)-1; i < ((_chunksize*_chunksize)-1); i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                int halfwayinthechunk = ((_chunksize -1) / 2) * _chunksize;
                for (int i = halfwayinthechunk + ((_chunksize -1)/2); i < halfwayinthechunk + _chunksize  ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize * _chunksize/2) + ((_chunksize-1)/2); i < (_chunksize*_chunksize)-1; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            return roadmap;
        }

        private ITile[] WestToSouth()
        {
            // First, if there is an even number of tiles in a chunk, we need two rows of roads. Otherwise, we'll only use one.
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                int halfwayinthechunk = ((_chunksize /2)-1) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + (_chunksize/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = halfwayinthechunk + _chunksize; i < halfwayinthechunk + _chunksize + (_chunksize/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize * _chunksize/2) + (_chunksize/2)-1; i < ((_chunksize*_chunksize)-1); i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                int halfwayinthechunk = ((_chunksize -1) / 2) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + ((_chunksize +1)/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize * _chunksize/2) + ((_chunksize-1)/2); i < (_chunksize*_chunksize)-1; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            return roadmap;
        }
        
        private ITile[] AllButNorth()
        {
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                int halfwayinthechunk = ((_chunksize /2)-1) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + (_chunksize*2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize * _chunksize/2) + (_chunksize/2)-1; i < ((_chunksize*_chunksize)-1); i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                int halfwayinthechunk = ((_chunksize -1) / 2) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + _chunksize ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize * _chunksize/2) + ((_chunksize-1)/2); i < (_chunksize*_chunksize)-1; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            
            return roadmap;
        }
        
        private ITile[] AllButSouth()
        {
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                int halfwayinthechunk = ((_chunksize /2)-1) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + (_chunksize*2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize/2)-1; i < (_chunksize* _chunksize)/2; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                int halfwayinthechunk = ((_chunksize -1) / 2) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + _chunksize ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize-1)/2; i < (_chunksize*_chunksize)/2; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            
            return roadmap;
        }
        
        private ITile[] AllButEast()
        {
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                int halfwayinthechunk = ((_chunksize /2)-1) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + (_chunksize/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = halfwayinthechunk + _chunksize; i < halfwayinthechunk + _chunksize + (_chunksize/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize/2)-1; i < (_chunksize* _chunksize)-1; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                int halfwayinthechunk = ((_chunksize -1) / 2) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + ((_chunksize +1)/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize-1)/2; i < (_chunksize*_chunksize)-1; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            return roadmap;
        }
        
        private ITile[] AllButWest()
        {
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                int halfwayinthechunk = ((_chunksize /2)-1) * _chunksize;
                for (int i = halfwayinthechunk + (_chunksize/2); i < halfwayinthechunk + _chunksize; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = halfwayinthechunk + _chunksize + (_chunksize/2); i < halfwayinthechunk + (_chunksize*2); i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize/2)-1; i < (_chunksize* _chunksize)-1; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                int halfwayinthechunk = ((_chunksize -1) / 2) * _chunksize;
                for (int i = halfwayinthechunk + ((_chunksize -1)/2); i < halfwayinthechunk + _chunksize  ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize-1)/2; i < (_chunksize*_chunksize)-1; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            return roadmap;
        }

        private ITile[] AllRoads ()
        {
            ITile[] roadmap = new ITile[_chunksize * _chunksize];
            if (_chunksize % 2 == 0)
            {
                int halfwayinthechunk = ((_chunksize /2)-1) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + (_chunksize*2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize/2)-1; i < (_chunksize* _chunksize)-1; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
                
            }
            else
            {
                int halfwayinthechunk = ((_chunksize -1) / 2) * _chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + _chunksize ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = (_chunksize-1)/2; i < (_chunksize*_chunksize)-1; i += _chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            return roadmap;
        }
    }
}