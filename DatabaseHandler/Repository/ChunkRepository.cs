using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Logging;
using WorldGeneration.Models;

namespace DatabaseHandler.Repository
{
    [ExcludeFromCodeCoverage]
    public class ChunkRepository : IChunkRepository
    {
        private readonly IDbConnection _connection;
        private readonly string _collection;

        [ExcludeFromCodeCoverage]
        public ChunkRepository(IDbConnection connection, string collection = "Chunks")
        {
            _connection = connection;
            _connection.SetConnectionString("Filename=.\\Chunks.db");
            _collection = collection;
        }

        [ExcludeFromCodeCoverage]
        public async Task<string> CreateAsync(Chunk obj)
        {
            var db = _connection.GetConnectionAsync();
            var result = await db.GetCollection<Chunk>(_collection).InsertAsync(obj);
            return result.RawValue.ToString();
        }

        [ExcludeFromCodeCoverage]
        public async Task<Chunk> ReadAsync(Chunk obj)
        {
            var db = _connection.GetConnectionAsync();
            var chunk = await db.GetCollection<Chunk>(_collection)
                .FindOneAsync(c => c.X.Equals(obj.X) && c.Y.Equals(obj.Y));
            return chunk;
        }

        [ExcludeFromCodeCoverage]
        public async Task<Chunk> UpdateAsync(Chunk obj)
        {
            var db = _connection.GetConnectionAsync();
            var results = await db.GetCollection<Chunk>(_collection).UpdateAsync(obj);
            return results ? obj : null;
        }

        [ExcludeFromCodeCoverage]
        public async Task<int> DeleteAsync(Chunk obj)
        {
            var db = _connection.GetConnectionAsync();
            var results = await db.GetCollection<Chunk>(_collection).DeleteManyAsync(chunk => chunk.X.Equals(obj.X) && chunk.Y.Equals(obj.Y));
            return results;
        }

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<Chunk>> GetAllAsync()
        {
            var db = _connection.GetConnection();
            var chunks = db.GetCollection<Chunk>(_collection).Query().ToList();
            return chunks;
        }

        [ExcludeFromCodeCoverage]
        public async Task<int> DeleteAllAsync()
        {
            var db = _connection.GetConnection();
            var result = db.GetCollection<Chunk>(_collection).DeleteAll();
            return result;
        }
    }
}