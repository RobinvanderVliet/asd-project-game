/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD project.
 
    This file is created by team: 3.
     
    Goal of this file: Prototype database storage.
     
*/

using System;
using System.Drawing;

namespace WorldGeneration
{
    class Class1
    {
        public Class1()
        {
            var grass = new TileType()
            {
                Symbol = '+',
                Color = Color.Green
            };
            var sand = new TileType()
            {
                Symbol = '=',
                Color = Color.Yellow
            };
            var water = new TileType()
            {
                Symbol = '~',
                Color = Color.DodgerBlue
            };
            var grassTile = new Tile()
            {
                GasLevel = 0,
                TileType = grass
            };
            var sandTile = new Tile()
            {
                GasLevel = 0,
                TileType = sand
            };
            var waterTile = new Tile()
            {
                GasLevel = 0,
                TileType = water
            };
            Tile[] tileMap = {grassTile, sandTile, waterTile, sandTile, 
                grassTile, grassTile, sandTile, sandTile,
                grassTile, grassTile, grassTile, grassTile,
                grassTile, grassTile, grassTile, grassTile
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
            
            var db = new Database();
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