/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD project.
 
    This file is created by team: 3.
     
    Goal of this file: Prototype database storage.
     
*/

using System;
using System.Drawing;

   Project name: ASD project.

   This file is created by team: 3.

   Goal of this file: Prototype database storage.

*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration
{
    class Class1
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
            var sand = new TileType()
            {
                symbol = '=',
                color = Color.Yellow
            };
            var water = new TileType()
            {
                symbol = '~',
                color = Color.DodgerBlue
            };
            var grassTile = new Tile()
            {
                gasLevel = 0,
                tileType = grass
            };
            var sandTile = new Tile()
            {
                gasLevel = 0,
                tileType = sand
            };
            var waterTile = new Tile()
            {
                gasLevel = 0,
                tileType = water
            };
            Tile[] tileMap = {grassTile, sandTile, waterTile, sandTile, 
                grassTile, grassTile, sandTile, sandTile,
                grassTile, grassTile, grassTile, grassTile,
                grassTile, grassTile, grassTile, grassTile
            };
            var chunk = new Chunk()
            {
                x = 0,
                y = 0,
                map = tileMap,
                rowSize = 4
            };
            var chunk2 = new Chunk()
            {
                x = 0,
                y = 1,
                map = tileMap,
                rowSize = 4
            };
            
            var db = new Database();
            db.deleteTileMap();
            db.insertChunkIntoDatabase(chunk);
            db.insertChunkIntoDatabase(chunk2);
            db.getAllChunks();
            var chunkA = db.getChunk(0,0);
            var chunkB = db.getChunk(0,1);
            chunkA.displayChunk();
            Console.WriteLine(chunkA.getTileCoordinatesInChunk(0)[0] + " " + chunkA.getTileCoordinatesInChunk(0)[1]); 
            Console.WriteLine(chunkA.getTileCoordinatesInChunk(1)[0] + " " + chunkA.getTileCoordinatesInChunk(1)[1]); 
            Console.WriteLine(chunkA.getTileCoordinatesInChunk(2)[0] + " " + chunkA.getTileCoordinatesInChunk(2)[1]); 
            Console.WriteLine(chunkA.getTileCoordinatesInChunk(3)[0] + " " + chunkA.getTileCoordinatesInChunk(3)[1]); 
            Console.WriteLine(chunkA.getTileCoordinatesInChunk(4)[0] + " " + chunkA.getTileCoordinatesInChunk(4)[1]); 
            Console.WriteLine(chunkA.getTileCoordinatesInChunk(5)[0] + " " + chunkA.getTileCoordinatesInChunk(5)[1]); 
            Console.WriteLine(chunkA.getTileCoordinatesInChunk(6)[0] + " " + chunkA.getTileCoordinatesInChunk(6)[1]); 
            Console.WriteLine(chunkA.getTileCoordinatesInChunk(7)[0] + " " + chunkA.getTileCoordinatesInChunk(7)[1]); 
            Console.WriteLine(chunkA.getTileCoordinatesInChunk(8)[0] + " " + chunkA.getTileCoordinatesInChunk(8)[1]); 
            Console.WriteLine(chunkA.getTileCoordinatesInChunk(9)[0] + " " + chunkA.getTileCoordinatesInChunk(9)[1]); 
            Console.WriteLine(" ------- "); 
            Console.WriteLine(chunkA.getTileCoordinatesInWorld(0)[0] + " " + chunkA.getTileCoordinatesInWorld(0)[1]);
            Console.WriteLine(chunkB.getTileCoordinatesInWorld(0)[0] + " " + chunkB.getTileCoordinatesInWorld(0)[1]);
        }
    }
}