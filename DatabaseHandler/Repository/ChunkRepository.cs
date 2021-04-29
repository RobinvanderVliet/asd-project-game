using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorldGeneration.Models;

namespace DatabaseHandler.Repository
{
    public class ChunkRepository : IChunkRepository
    {
        private readonly ILogger<ChunkRepository> _log;
        private readonly IDbConnection _connection;
        private readonly string _collection;

        public ChunkRepository(ILogger<ChunkRepository> log, IDbConnection connection, string collection = "Chunks")
        {
            _connection = connection;
            _log = log;
            _collection = collection;
        }
        
        public async Task<BsonValue> CreateAsync(Chunk obj)
        {
            try
            {
                var db = _connection.getConnectionASync();
                return await db.GetCollection<Chunk>(_collection).InsertAsync(obj);
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Chunk> ReadAsync(Chunk obj)
        {
            try
            {
                var db = _connection.getConnectionASync();
                return await db.GetCollection<Chunk>(_collection).FindOneAsync(chunk => chunk.X.Equals(obj.X) && chunk.Y.Equals(obj.Y));
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Chunk> UpdateAsync(Chunk oldObj, Chunk newObj)
        {
            try
            {
                //using var db = _connection.getConnectionASync();
                //return await db.GetCollection<Chunk>(_collection).UpdateAsync();
                throw new System.NotImplementedException();
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Chunk> DeleteAsync(Chunk obj)
        {
            try
            {
                //using var db = _connection.getConnectionASync();
                //return await db.GetCollection<Chunk>(_collection).DeleteAsync();
                throw new System.NotImplementedException();
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IList<Chunk>> GetAllAsync()
        {
            try
            {
                var db = _connection.getConnectionASync();
                return await db.GetCollection<Chunk>(_collection).Query().ToListAsync();
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<int> DeleteAllAsync()
        {
            try
            {
                var db = _connection.getConnectionASync();
                return await db.GetCollection<Chunk>(_collection).DeleteAllAsync();
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }
    }
}