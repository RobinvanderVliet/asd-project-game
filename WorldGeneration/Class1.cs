using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Json;
using LiteDB;

namespace WorldGeneration
{
    public class Class1
    {
        public Class1()
        {
            var lookingForX = 1;
            var lookingForY = 1;
            var grass = new TileType()
            {
                symbol = '+',
                color = Color.Green
            };
            var grassTile = new Tile()
            {
                gasLevel = 0,
                tileType = grass
            };
            Tile[,] tileMap = {{grassTile, grassTile}, {grassTile, grassTile}};
            var chunk = new Chunk()
            {
                x = 1,
                y = 2,
                map = tileMap
            };
            var chunk2 = new Chunk()
            {
                x = 1,
                y = 1,
                map = tileMap
            };
            
            var db = new Database();
            db.deleteTileMap();
            db.insertChunkIntoDatabase(chunk);
            db.insertChunkIntoDatabase(chunk2);
            //db.getAllChunks();
            using (var db2 = new LiteDatabase("C:\\Temp\\ChunkDatabase.db"))
            {
                var collection = db2.GetCollection<Chunk>("Chunks");

                var results = collection.Query()
                    .Where(chunk => chunk.x.Equals(lookingForX) && chunk.y.Equals(lookingForY))
                    .Select(queryOutput => new Chunk(queryOutput.x, queryOutput.y))
                    .ToList();
                /*
                var results = collection.Query()
                    .Where( chunk => chunk.x.Equals(lookingForX) && chunk.y.Equals(lookingForY) ) 
                    .Select(queryOutput => new {x = queryOutput.x, y = queryOutput.y })
                    .Limit(10)
                    .ToList();
                    */
                // foreach (var result in results)
                // {
                //     Console.WriteLine("waarde: " +  result);
                // }
            }/*

            // db.getChunk(1, 1);
            // var output = db.getChunk(1, 1);
            //Console.WriteLine("Chunk: " + output.x + " " + chunk.y);
            
            /*
            using (var db = new LiteDatabase(@"C:\Temp\ChunkDatabase.db"))
            {
                // Get a collection (or create, if doesn't exist)
                db.DropCollection("Chunks");
                var collection = db.GetCollection<Chunk>("Chunks");
            
                
                // Insert new customer document (Id will be auto-incremented)
                collection.Insert(chunk);
                collection.Insert(chunk2);
                
                var chunkOutput = collection.FindAll();
                
                Console.WriteLine("aantal waardes: " +  collection.Count());
                Console.WriteLine("aantal waardes: " +  collection.Count());
                var AAAAAAAAA = collection.Query()
                    .Where( chunk => chunk.x.Equals(lookingForX) && chunk.y.Equals(lookingForY) ) 
                    .Select(queryOutput => new {x = queryOutput.x, y = queryOutput.y })
                    .Limit(10)
                    .ToList();
                foreach (var result in AAAAAAAAA)
                {
                    Console.WriteLine("waarde: " +  result);
                }
        

            }
            */
        }
    }
}