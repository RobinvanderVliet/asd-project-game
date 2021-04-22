using System;
using WorldGeneration.Models;
using WorldGeneration.Models.BuildingTiles;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration
{
    public class Program
    {
        public Program()
        {
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
            var chunk2 = new Chunk()
            {
                X = 0,
                Y = 1,
                Map = tileMap,
                RowSize = 4
            };
            
            var db = new Database.Database();
            db.DeleteTileMap();
            db.InsertChunkIntoDatabase(chunk);
            db.InsertChunkIntoDatabase(chunk2);
            db.GetAllChunks();
            var chunkA = db.GetChunk(0,0);
            var chunkB = db.GetChunk(0,1);
            chunkA.DisplayChunk();
            Console.WriteLine(chunkA.GetTileCoordinatesInChunk(0)[0] + " " + chunkA.GetTileCoordinatesInChunk(0)[1]); 
            Console.WriteLine(chunkA.GetTileCoordinatesInChunk(1)[0] + " " + chunkA.GetTileCoordinatesInChunk(1)[1]); 
            Console.WriteLine(chunkA.GetTileCoordinatesInChunk(2)[0] + " " + chunkA.GetTileCoordinatesInChunk(2)[1]); 
            Console.WriteLine(chunkA.GetTileCoordinatesInChunk(3)[0] + " " + chunkA.GetTileCoordinatesInChunk(3)[1]); 
            Console.WriteLine(chunkA.GetTileCoordinatesInChunk(4)[0] + " " + chunkA.GetTileCoordinatesInChunk(4)[1]); 
            Console.WriteLine(chunkA.GetTileCoordinatesInChunk(5)[0] + " " + chunkA.GetTileCoordinatesInChunk(5)[1]); 
            Console.WriteLine(chunkA.GetTileCoordinatesInChunk(6)[0] + " " + chunkA.GetTileCoordinatesInChunk(6)[1]); 
            Console.WriteLine(chunkA.GetTileCoordinatesInChunk(7)[0] + " " + chunkA.GetTileCoordinatesInChunk(7)[1]); 
            Console.WriteLine(chunkA.GetTileCoordinatesInChunk(8)[0] + " " + chunkA.GetTileCoordinatesInChunk(8)[1]); 
            Console.WriteLine(chunkA.GetTileCoordinatesInChunk(9)[0] + " " + chunkA.GetTileCoordinatesInChunk(9)[1]); 
            Console.WriteLine(" ------- "); 
            Console.WriteLine(chunkA.GetTileCoordinatesInWorld(0)[0] + " " + chunkA.GetTileCoordinatesInWorld(0)[1]);
            Console.WriteLine(chunkB.GetTileCoordinatesInWorld(0)[0] + " " + chunkB.GetTileCoordinatesInWorld(0)[1]);
            Console.WriteLine(chunkB.GetTileCoordinatesInWorld(1)[0] + " " + chunkB.GetTileCoordinatesInWorld(1)[1]);
        }
    }
}