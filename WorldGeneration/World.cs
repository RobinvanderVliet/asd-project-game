using System;
using System.Collections.Generic;
using System.Linq;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
    public class World
    {
        private readonly int _chunkSize;
        private readonly int _seed;
        private IList<Chunk> _chunks;
        public IList<IPlayer> _players { get; set; }

        public World(IList<IPlayer> players, int chunkSize = 10, int seed = 0620520399)
        {
            _players = players;
            _chunkSize = chunkSize;
            _seed = seed;
        }

        public void LoadAreaForPlayer(IPlayer player, int viewDistance)
        {
            var chunksWithinLoadingRange = CalculateChunksToLoadForPlayer(player.CurrentPosition, viewDistance);
            var chunks = new List<Chunk>();
            var db = new Database.Database();

            foreach (var chunkXY in chunksWithinLoadingRange)
            {
                var chunk = db.GetChunk(chunkXY[0], chunkXY[1]);
                chunks.Add(chunk == null
                    ? GenerateNewChunk(chunkXY[0], chunkXY[1])
                    : db.GetChunk(chunkXY[0], chunkXY[1]));
            }

            DisplayMap(player, viewDistance, chunks);
        }

        private IEnumerable<int[]> CalculateChunksToLoadForPlayer(int[] playerLocation, int viewDistance)
        {
            var maxX = (playerLocation[0] + viewDistance * 2 + _chunkSize) / _chunkSize;
            var minX = (playerLocation[0] - viewDistance * 2 - _chunkSize * 2) /
                       _chunkSize; // chunks beginnen links bovenin, dus daarom *2
            var maxY = (playerLocation[1] + viewDistance * 2 + _chunkSize * 2) / _chunkSize;
            var minY = (playerLocation[1] - viewDistance * 2 - _chunkSize) / _chunkSize;
            var chunksWithinLoadingRange = new List<int[]>();


            for (var x = minX; x <= maxX; x++)
            for (var y = minY; y < maxY; y++)
                chunksWithinLoadingRange.Add(new[] {x, y});
            return chunksWithinLoadingRange;
        }

        private void DisplayMap(IPlayer currentPlayer, int viewDistance, IReadOnlyCollection<Chunk> chunks)
        {
            for (var y = currentPlayer.CurrentPosition[1]; y < viewDistance * 2 + 1; y++)
            for (var x = currentPlayer.CurrentPosition[0]; x < viewDistance * 2 + 1; x++)
            {
                var chunk = chunks.FirstOrDefault(chunk =>
                    chunk.X * _chunkSize <= x && chunk.X * _chunkSize > x - _chunkSize && chunk.Y * _chunkSize >= y &&
                    chunk.Y * _chunkSize < y + _chunkSize);
                if (chunk == null) throw new Exception("this chunk should not be null");
                var tile = chunk.Map[chunk.GetPositionInTileArrayByWorldCoordinates(x, y)];
                Console.Write(DisplaySymbol(currentPlayer, tile));
                if (x == viewDistance * 2) Console.WriteLine("");
            }
        }

        private Chunk GenerateNewChunk(int x, int y)
        {
            return NoiseMapGenerator.GenerateChunk(x, y, _chunkSize, _seed);
        }
        
        private bool IsPlayerOnTile(ITile tile, IPlayer player)
        {
            return tile.X == player.CurrentPosition[0] && tile.Y == player.CurrentPosition[1];
        }

        private string DisplaySymbol(IPlayer currentPlayer, ITile tile)
        {
            if (IsPlayerOnTile(tile, currentPlayer))
            {
                return " " + currentPlayer.Symbol;
            }
            foreach (var player in _players)
            {
                if (player.Name == currentPlayer.Name) continue;
                // moet nog een check komen of het friendly of enemy player is
                player.Symbol = CharacterSymbol.ENEMY_PLAYER;
                return " " + player.Symbol;
            }
            return " " + tile.Symbol;
            }
        
        public void MovePlayer(string playerName, int positionX, int positionY)
        {
            foreach (var player in _players)
            {
                if (player.Name != playerName) continue;
                if (player.CurrentPosition[0] == positionX && player.CurrentPosition[1] == positionY)
                {
                    // Player is trying to move to the exact same position.
                }
                else
                {
                    player.CurrentPosition = new[] {positionX, positionY};
                    // Update the map to show new position.
                }
            }
        }
    }
}