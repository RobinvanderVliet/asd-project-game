using System;
using System.Collections;
using System.Collections.Generic;
using WorldGeneration.Models;
using WorldGeneration.Models.Enums;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration
{
    public static class RoadPresetFactory
    {
        // Provides a set of preset road "templates" that connect the correct sides.
        private static Hashtable _singletonRoadPresets;
        
        public static ITile[] GetRoadPreset (int chunksize, CompassDirections directions)
        {
            if (_singletonRoadPresets == null)
            {
                _singletonRoadPresets = GenerateRoadPresets(chunksize);
            }
            ITile[] result = _singletonRoadPresets[directions] as ITile[];
            return result;
        }

        static Hashtable GenerateRoadPresets(int chunksize)
        {
            Hashtable roadpresets = new Hashtable();
            roadpresets.Add(CompassDirections.NoRoads, NoRoads(chunksize));
            roadpresets.Add(CompassDirections.EastOnly, EastOnly(chunksize));
            roadpresets.Add(CompassDirections.WestOnly, WestOnly(chunksize));
            roadpresets.Add(CompassDirections.NorthOnly, NorthOnly(chunksize));
            roadpresets.Add(CompassDirections.SouthOnly, SouthOnly(chunksize));
            roadpresets.Add(CompassDirections.NorthToSouth, NorthToSouth(chunksize));
            roadpresets.Add(CompassDirections.EastToWest, EastToWest(chunksize));
            return roadpresets;
        }

        private static ITile[] NoRoads(int chunksize)
        {
            // If there are no roads, return an empty roadmap.
            ITile[] roadmap = new ITile[chunksize * chunksize];
            return roadmap;
        }
        
        private static ITile[] NorthToSouth (int chunksize)
        {
            // First, if there is an even number of tiles in a chunk, we need two rows of roads. Otherwise, we'll only use one.
            ITile[] roadmap = new ITile[chunksize * chunksize];
            if (chunksize % 2 == 0)
            {
                for (int i = (chunksize/2)-1; i < (chunksize* chunksize)-1; i += chunksize)
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
                for (int i = (chunksize-1)/2; i < (chunksize*chunksize)-1; i += chunksize)
                {
                    // Simple example using chunksize of 9:
                    // Half of 8 is 4, so the loop begins at 4. (Because our list begins at 0, this is the fifth tile).
                    // Tile 4 is turned into a street tile. Then, iterating by 9, the next tile is 13, then so on.
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            
            return roadmap;
        }
        
        
        private static ITile[] EastToWest (int chunksize)
        {
            // First, if there is an even number of tiles in a chunk, we need two rows of roads. Otherwise, we'll only use one.
            ITile[] roadmap = new ITile[chunksize * chunksize];
            if (chunksize % 2 == 0)
            { 
                // Example using chunksize of 8:
                // half of chunksize = 4;
                // 3 * 8 = 24. Because we start at 0, this is the first tile of the fourth row.
                // fill the next two rows.
                int halfwayinthechunk = ((chunksize /2)-1) * chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + (chunksize*2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
            }
            else
            {
                // Simple example using chunksize of 9:
                // half of chunksize = 4;
                // 4 * 9 = 36. This is the first tile of the fifth row.
                // Turn all the tiles on that row into street tiles. Again the x and y are placeholders that will be overwritten.
                
                int halfwayinthechunk = ((chunksize -1) / 2) * chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + chunksize ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
            }
            
            return roadmap;
        }
        
        private static ITile[] EastOnly (int chunksize)
        {
            // First, if there is an even number of tiles in a chunk, we need two rows of roads. Otherwise, we'll only use one.
            ITile[] roadmap = new ITile[chunksize * chunksize];
            if (chunksize % 2 == 0)
            { 
                // Example using chunksize of 8:
                // half of chunksize = 4;
                // 3 * 8 = 24. Because we start at 0, this is the first tile of the fourth row.
                // Instead of filling the next two rows, only fill the first half of both rows. The easiest ways to do this is do two loops.
                int halfwayinthechunk = ((chunksize /2)-1) * chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + (chunksize/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = halfwayinthechunk + chunksize; i < halfwayinthechunk + chunksize + (chunksize/2) ; i++)
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
                int halfwayinthechunk = ((chunksize -1) / 2) * chunksize;
                for (int i = halfwayinthechunk; i < halfwayinthechunk + ((chunksize +1)/2) ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
            }
            return roadmap;
        }
        
        private static ITile[] WestOnly (int chunksize)
        {
            // First, if there is an even number of tiles in a chunk, we need two rows of roads. Otherwise, we'll only use one.
            ITile[] roadmap = new ITile[chunksize * chunksize];
            if (chunksize % 2 == 0)
            {
                // Uses similar logic to EastOnly, but instead of filling the first half of the row, fill the second half.
                int halfwayinthechunk = ((chunksize /2)-1) * chunksize;
                for (int i = halfwayinthechunk + (chunksize/2); i < halfwayinthechunk + chunksize; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
                for (int i = halfwayinthechunk + chunksize + (chunksize/2); i < halfwayinthechunk + (chunksize*2); i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
            }
            else
            {
                int halfwayinthechunk = ((chunksize -1) / 2) * chunksize;
                for (int i = halfwayinthechunk + ((chunksize -1)/2); i < halfwayinthechunk + chunksize  ; i++)
                {
                    roadmap[i] = new StreetTile(0, 0);
                }
            }
            return roadmap;
        }
        private static ITile[] NorthOnly (int chunksize)
        {
            // First, if there is an even number of tiles in a chunk, we need two rows of roads. Otherwise, we'll only use one.
            ITile[] roadmap = new ITile[chunksize * chunksize];
            if (chunksize % 2 == 0)
            {
                for (int i = (chunksize/2)-1; i < (chunksize* chunksize)/2; i += chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                for (int i = (chunksize-1)/2; i < (chunksize*chunksize)/2; i += chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            
            return roadmap;
        }
        private static ITile[] SouthOnly (int chunksize)
        {
            // First, if there is an even number of tiles in a chunk, we need two rows of roads. Otherwise, we'll only use one.
            ITile[] roadmap = new ITile[chunksize * chunksize];
            if (chunksize % 2 == 0)
            {
                for (int i = (chunksize * chunksize/2) + (chunksize/2)-1; i < ((chunksize*chunksize)-1); i += chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                    roadmap[i + 1] = new StreetTile(0, 0);
                }
            }
            else
            {
                for (int i = (chunksize * chunksize/2) + ((chunksize-1)/2); i < (chunksize*chunksize)-1; i += chunksize)
                {
                    roadmap[i] = new StreetTile(0,0);
                }
            }
            
            return roadmap;
        }
    }
}