using System;
using System.Collections.Generic;
using WorldGeneration.Database;
using WorldGeneration.Models;
using WorldGeneration.Models.BuildingTiles;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration
{
    public class Map
    {
        private List<Chunk> _chunks;
        private int chunkSize;

        public Map(int chunkSize = 4)
        {
            this.chunkSize = chunkSize;
        }

        public void LoadArea(int[] playerLocation, int viewDistance)
        {
            var chunksWithinLoadingRange = CalculateChunksToLoad(playerLocation, viewDistance);
            var chunks = new List<Chunk>();
            var db = new Database.Database();
            
            foreach (var chunk in chunksWithinLoadingRange)
            {
                Console.WriteLine(chunk[0] + " " + chunk[1]);
                try
                {
                    chunks.Add(db.GetChunk(chunk[0], chunk[1]));
                }
                catch (ChunkNotFoundException e)
                {
                    Console.WriteLine("no chunk found, generating new chunk");
                    chunks.Add(generateNewChunk(chunk[0], chunk[1], chunkSize));
                }
            }
            DisplayMap(viewDistance, chunks);
        }

        public List<int[]> CalculateChunksToLoad(int[] playerLocation, int viewDistance)
        {
            var maxX = (playerLocation[0] + viewDistance + chunkSize) / chunkSize;
            var minX = (playerLocation[0] - viewDistance - (chunkSize * 2)) / chunkSize; // chunks beginnen links bovenin, dus daarom *2
            var maxY = (playerLocation[1] + viewDistance + (chunkSize * 2)) / chunkSize;
            var minY = (playerLocation[1] - viewDistance - chunkSize) / chunkSize;
            var chunksWithinLoadingRange = new List<int[]>();
            

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    chunksWithinLoadingRange.Add(new int[]{x,y});
                }
            }
            return chunksWithinLoadingRange;
        }

        public void DisplayMap(int viewDistance, List<Chunk> chunks)
        {
            for (int y = 0; y < (viewDistance * 2 + 1); y++)
            {
                for (int x = 0; x < (viewDistance * 2 + 1); x++)
                {
                    //the chunk this tile will be in is within tile y +chunksize and x -chunksize
                    var chunk = chunks.Find(chunk =>
                        Equals(chunk.X <= x && chunk.X >= x - chunkSize && chunk.Y >= y && chunk.Y <= y - chunkSize));
                    if(chunk.Equals(null))
                    {
                        throw new Exception();
                    }
                }
            }
        }

        private Chunk generateNewChunk(int x, int y, int rowSize)
        {
            //TODO: replace placeholder function
            var tileMap = new ITile[] {
                new DirtTile(), new DirtTile(), new StreetTile(), new StreetTile(), 
                new WaterTile(), new WaterTile(), new WaterTile(), new WaterTile(), 
                new StreetTile(), new StreetTile(), new StreetTile(), new StreetTile(),
                new WallTile(), new DoorTile(), new WallTile(), new GrassTile()
            };
            var chunk = new Chunk()
            {
                X = 0,
                Y = 0,
                Map = tileMap,
                RowSize = 4
            };
            return chunk;
        }
    }
}       