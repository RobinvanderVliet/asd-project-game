using DataTransfer.DTO.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
    public class Map
    {
        private readonly int _chunkSize;
        private readonly int _seed;
        private List<Chunk> _chunks; // NOT readonly, don't listen to the compiler
        private readonly DatabaseFunctions.Database _db;
        private List<int[]> _chunksWithinLoadingRange;

        private INoiseMapGenerator _noiseMapGenerator;

        public Map(
            INoiseMapGenerator noiseMapGenerator
            , DatabaseFunctions.Database db
            , int chunkSize
            , int seed
        )
        {
            _chunkSize = chunkSize;
            _db = db;
            _chunks = new List<Chunk>();
            _seed = seed;
            _noiseMapGenerator = noiseMapGenerator;
        }

        // checks if there are new chunks that have to be loaded
        private void LoadArea(int playerX, int playerY, int viewDistance)
        {
            _chunksWithinLoadingRange = CalculateChunksToLoad(playerX, playerY, viewDistance);
            ForgetUnloadedChunks();
            foreach (var chunkXY in _chunksWithinLoadingRange)
            {
                if (!_chunks.Exists(chunk => chunk.X == chunkXY[0] && chunk.Y == chunkXY[1]))
                { // chunk isn't loaded in local memory yet
                    var chunk = _db.GetChunk(chunkXY[0], chunkXY[1]);
                    _chunks.Add(chunk == null
                        ? GenerateNewChunk(chunkXY[0], chunkXY[1])
                        : _db.GetChunk(chunkXY[0], chunkXY[1]));
                }
            }
        }

        // cleanup function to forget chunks out of loading range
        private void ForgetUnloadedChunks()
        {
            /*
            foreach (var loadedChunk in _chunks)
            {
                if (!_chunksWithinLoadingRange.Exists(
                    chunkWithinLoadingRange =>
                        chunkWithinLoadingRange[0] == loadedChunk.X
                        && chunkWithinLoadingRange[1] == loadedChunk.Y))
                {
                    if (!_chunks.Remove(loadedChunk))
                    {
                        throw new Exception("Failed to remove chunk from loaded chunks");
                    }
                }
            }
            */
        }

        private List<int[]> CalculateChunksToLoad(int playerX, int playerY, int viewDistance)
        {
            // viewDistance * 2 is to get a full screen
            // , + playerX to get to the right location
            // , + chunksize to add some loading buffer
            // , / chunksize to convert tile coordinates to world coordinates
            // same for the other variables
            var maxX = (playerX + viewDistance * 2 + _chunkSize) / _chunkSize;
            var minX = (playerX - viewDistance * 2 - _chunkSize) / _chunkSize;
            var maxY = (playerY + viewDistance * 2 + _chunkSize) / _chunkSize + 1;
            var minY = (playerY - viewDistance * 2 - _chunkSize) / _chunkSize;
            var chunksWithinLoadingRange = new List<int[]>();

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y < maxY; y++)
                {
                    chunksWithinLoadingRange.Add(new[] { x, y });
                }
            }
            return chunksWithinLoadingRange;
        }

        public void DisplayMap(MapCharacterDTO currentPlayer, int viewDistance, IList<MapCharacterDTO> characters)
        {
            var playerX = currentPlayer.XPosition;
            var playerY = currentPlayer.YPosition;
            LoadArea(playerX, playerY, viewDistance);
            for (var y = (playerY + viewDistance); y > ((playerY + viewDistance) - (viewDistance * 2) - 1); y--)
            {
                for (var x = (playerX - viewDistance); x < ((playerX - viewDistance) + (viewDistance * 2) + 1); x++)
                {
                    var tile = GetLoadedTileByXAndY(x, y);
                    Console.Write($" {GetDisplaySymbol(currentPlayer, tile, characters)}");
                }
                Console.WriteLine("");
            }
        }

        private string GetDisplaySymbol(MapCharacterDTO currentPlayer, ITile tile, IList<MapCharacterDTO> characters)
        {
            bool currentPlayerOnTile = IsPlayerOnTile(tile, currentPlayer);
            if (currentPlayerOnTile)
            {
                return currentPlayer.Symbol;
            }
            foreach (var characterOnTile in characters.Where(character => character.XPosition == tile.XPosition && character.YPosition - 1 == tile.YPosition))
            {
                if (characterOnTile.Symbol == CharacterSymbol.FRIENDLY_PLAYER)
                {
                    if (characterOnTile.Team != currentPlayer.Team || characterOnTile.Team == 0)
                    {
                        return CharacterSymbol.ENEMY_PLAYER;
                    }
                    return CharacterSymbol.FRIENDLY_PLAYER;
                }
                return characterOnTile.Symbol;
            }
            return tile.Symbol;
        }

        private bool IsPlayerOnTile(ITile tile, MapCharacterDTO player)
        {
            return tile.XPosition == player.XPosition && tile.YPosition == player.YPosition - 1;
        }

        private Chunk GenerateNewChunk(int chunkX, int chunkY)
        {
            var chunk = _noiseMapGenerator.GenerateChunk(chunkX, chunkY, _chunkSize, _seed);
            _db.InsertChunkIntoDatabase(chunk);
            return chunk;
        }

        private Chunk GetChunkForTileXAndY(int x, int y)
        {
            var chunk = _chunks.FirstOrDefault(chunk =>
                chunk.X * _chunkSize <= x
                && chunk.X * _chunkSize > x - _chunkSize
                && chunk.Y * _chunkSize >= y &&
                chunk.Y * _chunkSize < y + _chunkSize);

            if (chunk == null)
            {
                throw new Exception("Tried to find a chunk that has not been loaded");
            }
            return chunk;
        }

        public void DeleteMap()
        {
            _db.DeleteTileMap();
        }

        // find a LOADED tile by the coordinates
        public ITile GetLoadedTileByXAndY(int x, int y)
        {
            var tile = GetChunkForTileXAndY(x, y).GetTileByWorldCoordinates(x, y);
            return tile;
        }
    }
}