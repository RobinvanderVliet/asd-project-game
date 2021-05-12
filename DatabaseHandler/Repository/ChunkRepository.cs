using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using LiteDB.Async;
using WorldGeneration.Models;

namespace DatabaseHandler.Repository
{
    [ExcludeFromCodeCoverage]
    public class ChunkRepository : IChunkRepository
    {
        private readonly IDbConnection _connection;
        private readonly string _collection;
        private readonly ILiteDatabaseAsync _db;

        [ExcludeFromCodeCoverage]
        public ChunkRepository(IDbConnection connection, string collection = "Chunks")
        {
            _connection = connection;
            _connection.SetConnectionString("Filename=.\\Chunks.db");
            _collection = collection;
            _db = _connection.GetConnectionAsync();
        }

        [ExcludeFromCodeCoverage]
        public async Task<string> CreateAsync(Chunk obj)
        {
            var result = await _db.GetCollection<Chunk>(_collection).InsertAsync(obj).ConfigureAwait(false);
            return result.RawValue.ToString();
        }

        [ExcludeFromCodeCoverage]
        public async Task<Chunk> ReadAsync(Chunk obj)
        {
            var chunk = await _db.GetCollection<Chunk>(_collection)
                .FindOneAsync(c => c.X.Equals(obj.X) && c.Y.Equals(obj.Y)).ConfigureAwait(false);
            return chunk;
        }

        [ExcludeFromCodeCoverage]
        public async Task<Chunk> UpdateAsync(Chunk obj)
        {
            var results = await _db.GetCollection<Chunk>(_collection).UpdateAsync(obj).ConfigureAwait(false);
            return results ? obj : null;
        }

        [ExcludeFromCodeCoverage]
        public async Task<int> DeleteAsync(Chunk obj)
        {
            var results = await _db.GetCollection<Chunk>(_collection)
                .DeleteManyAsync(chunk => chunk.X.Equals(obj.X) && chunk.Y.Equals(obj.Y)).ConfigureAwait(false);
            return results;
        }

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<Chunk>> GetAllAsync()
        {
            var chunks = await _db.GetCollection<Chunk>(_collection).Query().ToListAsync().ConfigureAwait(false);
            return chunks;
        }

        [ExcludeFromCodeCoverage]
        public async Task<int> DeleteAllAsync()
        {
            var result = await _db.GetCollection<Chunk>(_collection).DeleteAllAsync().ConfigureAwait(false);
            return result;
        }
    }
}