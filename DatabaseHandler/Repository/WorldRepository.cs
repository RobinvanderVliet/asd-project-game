using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Logging;
using WorldGeneration.Models;

namespace DatabaseHandler.Repository
{
    public class WorldRepository : IRepository<Chunk>
    {
        private readonly ILogger<WorldRepository> _log;
        private readonly IDbConnection _connection;
        private readonly string _collection;

        public WorldRepository(IDbConnection connection, ILogger<WorldRepository> log, string collection)
        {
            _connection = connection;
            _log = log;
            _collection = collection;
        }
        
        public async Task<Chunk> CreateASync(Chunk obj)
        {
            try
            {
                using var db = _connection.getConnectionASync();
                await db.GetCollection<Chunk>(_collection).InsertAsync(obj);
                return obj;
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Chunk> ReadASync(Chunk obj)
        {
            try
            {
                using var db = _connection.getConnectionASync();
                return await db.GetCollection<Chunk>(_collection).FindOneAsync(chunk => chunk.X.Equals(obj.X) && chunk.Y.Equals(obj.Y));
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Chunk> UpdateASync(Chunk oldObj, Chunk newObj)
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

        public async Task<Chunk> DeleteASync(Chunk obj)
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

        public async Task<IList<Chunk>> GetAllASync()
        {
            try
            {
                using var db = _connection.getConnectionASync();
                return await db.GetCollection<Chunk>(_collection).Query().ToListAsync();
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<int> DeleteAllASync()
        {
            try
            {
                using var db = _connection.getConnectionASync();
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