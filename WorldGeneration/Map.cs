using System;
using System.Collections.Generic;
using System.Linq;
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
            
            Console.WriteLine("lengte Chunk coordinates list: " + chunksWithinLoadingRange.Count);
            foreach (var chunkXY in chunksWithinLoadingRange)
            {
                Console.WriteLine("Retrieving chunk " + chunkXY[0] + " " + chunkXY[1]);
                var chunk = db.GetChunk(chunkXY[0], chunkXY[1]);
                if (chunk == null)
                {
                    Console.WriteLine("no chunk found, generating new chunk");
                    chunks.Add(generateNewChunk(chunkXY[0], chunkXY[1], chunkSize));
                }
                else
                {
                    chunks.Add(db.GetChunk(chunkXY[0], chunkXY[1]));
                }
            }
            Console.WriteLine("lengte Chunks list: " + chunks.Count);
            DisplayMap(0, 0, viewDistance, chunks);
        }

        public List<int[]> CalculateChunksToLoad(int[] playerLocation, int viewDistance)
        {
            var maxX = (playerLocation[0] + viewDistance * 2 + chunkSize ) / chunkSize;
            var minX = (playerLocation[0] - viewDistance * 2 - (chunkSize * 2)) / chunkSize; // chunks beginnen links bovenin, dus daarom *2
            var maxY = (playerLocation[1] + viewDistance * 2 + (chunkSize * 2)) / chunkSize;
            var minY = (playerLocation[1] - viewDistance * 2 - chunkSize) / chunkSize;
            var chunksWithinLoadingRange = new List<int[]>();
            

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    chunksWithinLoadingRange.Add(new int[]{x,y});
                    Console.WriteLine("Chunks that will be loaded: " + new int[]{x,y}[0] + " " + new int[]{x,y}[1]);
                }
            }
            return chunksWithinLoadingRange;
        }

        public void DisplayMap(int playerx, int playery, int viewDistance, List<Chunk> chunks)
        {
            for (int y = playery; y < (viewDistance * 2 + 1); y++)
            {
                for (int x = playerx; x < (viewDistance * 2 + 1); x++)
                {
                    /*
                    Console.WriteLine("!!!!!!!!!!This is not the x you're looking for: " + x + ", and this is not the y you're looking for: " + y);
                    foreach (var chunkAAAAA in chunks)
                    {
                        Console.WriteLine("x: " + chunkAAAAA.X+ ", y: " +  + chunkAAAAA.Y);
                    }*/
                    //the chunk this tile will be in is within tile y +chunksize and x -chunksize
                    /*chunks.ForEach(chunk =>
                    {
                        Console.WriteLine(
                        "   1: " + (chunk.X * chunkSize <= x) +
                        "   2: " + (chunk.X * chunkSize > x - chunkSize) +
                        "   3: " + (chunk.Y * chunkSize >= y) +
                        "   4: " + (chunk.Y * chunkSize < y + chunkSize) +
                        "   5: " + (chunk.X * chunkSize <= x && chunk.X * chunkSize > x - chunkSize && chunk.Y * chunkSize >= y && chunk.Y * chunkSize < y + chunkSize));
                        if ((chunk.X * chunkSize <= x && chunk.X * chunkSize > x - chunkSize &&
                             chunk.Y * chunkSize >= y && chunk.Y * chunkSize < y + chunkSize))
                        {
                            Console.WriteLine("It's this one: " + chunk.X + chunk.Y);
                        }
                    });*/
                    
                    var chunk = chunks.FirstOrDefault(chunk =>
                        chunk.X * chunkSize <= x && chunk.X * chunkSize > x - chunkSize && chunk.Y * chunkSize >= y && chunk.Y * chunkSize < y + chunkSize);
                    if(chunk == null)
                    {
                        throw new Exception("this chunk should not be null brah");
                    }
                    Console.Write(chunk.Map[chunk.getPositionInTileArrayByWorldCoordinates(x, y)].Symbol);
                    if (x == viewDistance * 2)
                    {
                        Console.WriteLine("");
                    }
                    
                    //Console.WriteLine("Pos in array: " + chunk.getPositionInTileArrayByWorldCoordinates(x, y));
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
                X = x,
                Y = y,
                Map = tileMap,
                RowSize = 4
            };
            return chunk;
        }
    }
}       