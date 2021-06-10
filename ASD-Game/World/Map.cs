using System;
using System.Collections.Generic;
using System.Linq;
using ASD_Game.World.Helpers;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World
{
    public class Map : IMap
    { 
        // This class is documented in chapter "Design Sub-System World Generation" of the SDD.
        private readonly int _chunkSize;
        private IList<Chunk> _chunks;
        private readonly INoiseMapGenerator _noiseMapGenerator;

        public Map(
            INoiseMapGenerator noiseMapGenerator
            , int chunkSize
            , IList<Chunk> chunks = null
        )
        {
            if (chunkSize < 1)
            {
                throw new InvalidOperationException("Chunksize smaller than 1.");
            }
            _chunkSize = chunkSize;
            _chunks = chunks ?? new List<Chunk>();
            _noiseMapGenerator = noiseMapGenerator;
        }

        public void LoadArea(int playerX, int playerY, int viewDistance) 
        { 
            // Gets a list of chunks it has to load. Then generates the ones it can't find in the list of loaded chunks yet.
            var chunksWithinLoadingRange = GetListOfChunksWithinLoadingRange(playerX, playerY, viewDistance);
            foreach (var chunkCoordinates in chunksWithinLoadingRange)
            {
                if (_chunks.Any(chunk => chunk.X == chunkCoordinates[0] && chunk.Y == chunkCoordinates[1])) continue;
                {
                    _chunks.Add(GenerateNewChunk(chunkCoordinates[0], chunkCoordinates[1]));
                }
            }
        }

        private List<int[]> GetListOfChunksWithinLoadingRange(int playerX, int playerY, int viewDistance)
        {
            var maxX = (playerX + viewDistance * 2 + _chunkSize) / _chunkSize; 
            var minX = (playerX - viewDistance * 2 - _chunkSize) / _chunkSize;
            var maxY = (playerY + viewDistance * 2 + _chunkSize) / _chunkSize + 1;
            var minY = (playerY - viewDistance * 2 - _chunkSize) / _chunkSize;
            var chunksWithinLoadingRange = new List<int[]>();

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y < maxY; y++)
                {
                    chunksWithinLoadingRange.Add(new[] {x, y});
                }
            }
            return chunksWithinLoadingRange;
        }

        public char[,] GetCharArrayMapAroundCharacter(Character centerCharacter, int viewDistance, List<Character> allCharacters)
        { 
            // Returns a 2d char array centered around a character.
            // The view distance is how far the map is rendered to all sides from the character.
            // The character list should contain all characters you may wish to display.
            if (viewDistance < 0)
            {
                throw new InvalidOperationException("viewDistance smaller than 0.");
            }
            
            var tileArray = new char[viewDistance * 2 + 1, viewDistance * 2 + 1]; // The +1 is because the view window is the view distance to each side, plus the tile the character itself uses.
            LoadArea(centerCharacter.XPosition, centerCharacter.YPosition, viewDistance);
            // y is counting down because of writing top to bottom,
            // x is counting up because writing left to right.
            // x and y are NOT real world coordinates.
            for (var y = tileArray.GetLength(0) - 1; y >= 0; y--)
            {
                for (var x = 0; x < tileArray.GetLength(1); x++)
                {
                    var currentTile = GetLoadedTileByXAndY(x + (centerCharacter.XPosition - viewDistance), (centerCharacter.YPosition + viewDistance) - y);
                    tileArray[y, x] = GetDisplaySymbolForSpecificTile(currentTile, allCharacters).ToCharArray()[0];
                }
            }
            return tileArray;
        }
        
        private string GetDisplaySymbolForSpecificTile(ITile tile, List<Character> characters)
        { 
            // Returns a string with whichever symbol it can find first in this order:
            // 1. Character symbol, 2 Item symbol (shows a chest tile), 3 Tile symbol.
            var characterOnTile = characters.FirstOrDefault(character => character.XPosition == tile.XPosition && character.YPosition == tile.YPosition);
            if(characterOnTile != null)
            {
                return characterOnTile.Symbol;
            }
            if (tile.ItemsOnTile.Count != 0)
            {
                return TileSymbol.CHEST;
            }
            return tile.Symbol;                    
        }

        private Chunk GenerateNewChunk(int chunkX, int chunkY)
        { 
            // Calls upon the noise map generator to generate a chunk based on a seed. This will ensure the chunk is the same for a given seed every time you generate it.
            return _noiseMapGenerator.GenerateChunk(chunkX, chunkY, _chunkSize);
        }

        private Chunk GetLoadedChunkForTileXAndY(int x, int y)
        { 
            // Tries to find a chunk in the already generated chunks. If it cannot be found it returns null.
            // It works by converting each chunk's chunk coordinates to standard coordinates. This is done by multiplying the chunk coordinates by the size of the chunk.
            // Then it checks if the x and y of the coordinates you're looking for fall within the chunk.
            return _chunks.FirstOrDefault(chunk =>
                chunk.X * _chunkSize <= x 
                && chunk.X * _chunkSize > x - _chunkSize 
                && chunk.Y * _chunkSize >= y &&
                chunk.Y * _chunkSize < y + _chunkSize);
        }
        
        public void DeleteMap()
        {
            _chunks.Clear();
        }
        
        public ITile GetLoadedTileByXAndY(int x, int y)
        {
            var chunkHelper = new ChunkHelper(GetLoadedChunkForTileXAndY(x, y));
            return chunkHelper.GetTileByWorldCoordinates(x, y);
        }
    }
}