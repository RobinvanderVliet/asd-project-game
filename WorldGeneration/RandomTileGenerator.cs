/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 3.
     
    Goal of this file: Calls for generation method of map with random tiles and write the tiles in lines to console.
     
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
    public class RandomTileGenerator
    {
        TileMap tileMap = new TileMap();
        IList<Tile> map = null;
        ITile currentTile;

        public void generate()
        {
            map = tileMap.generateBaseTerrain();

            string tileLine;

            for (var x = 0; x < 40; x++)
            {
                tileLine = "";
                for (var y = 0; y < 80; y++)
                {
                    currentTile = map.Where(tile => (tile.X == x) && (tile.Y == y)).FirstOrDefault();
                    tileLine += currentTile.Symbol;
                }
                Console.WriteLine(tileLine);
            }
        }
        
    }
}
