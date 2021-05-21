using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DatabaseHandler.Poco;
using LiteDB;
using LiteDB.Async;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DatabaseHandler.Repository
{
    [ExcludeFromCodeCoverage]
    public class Repository<T> : IRepository<T>
    {
        private readonly string _collection;
        private readonly ILiteDatabaseAsync _db;
        private readonly ILogger<Repository<T>> _log;

        [ExcludeFromCodeCoverage]
        public Repository(IDbConnection connection, string collection = null)
        {
            _collection = collection ?? typeof(T).Name;
            _db = connection.GetConnectionAsync();
            _log = new NullLogger<Repository<T>>();
        }

        [ExcludeFromCodeCoverage]
        public async Task<BsonValue> CreateAsync(T obj)
        {
            var result = await _db.GetCollection<T>(_collection).InsertAsync(obj);
            return result;
        }

        [ExcludeFromCodeCoverage]
        public async Task<T> ReadAsync(T obj)
        {
            var chunk = await _db.GetCollection<T>(_collection)
                .FindOneAsync(c => c.Equals(obj));
            return chunk;
        }

        [ExcludeFromCodeCoverage]
        public async Task<int> UpdateAsync(T obj)
        {
            var results = await _db.GetCollection<T>(_collection).UpdateAsync(obj);
            return results ? 1 : throw new InvalidOperationException($"Object op type {typeof(T)} does not exist in database.");
        }

        [ExcludeFromCodeCoverage]
        public async Task<int> DeleteAsync(T obj)
        {
            var results = await _db.GetCollection<T>(_collection)
                .DeleteManyAsync(c => c.Equals(obj));
            return results;
        }

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await _db.GetCollection<T>(_collection).Query().ToListAsync();
            return result;
        }
        

        [ExcludeFromCodeCoverage]
        public async Task<int> DeleteAllAsync()
        {
            var result = await _db.GetCollection<T>(_collection).DeleteAllAsync();
            return result;
        }

        public async Task<IEnumerable<PlayerPoco>> GetAllPoco()
        {
            var result = await _db.GetCollection<PlayerPoco>(_collection).Query().ToListAsync();
            return result; 
        }
        
        
        public async Task<Boolean> UpdateAsyncPlayer(string playerGUID, int newPosX, int newPosY)
        {
            var results =  _db.GetCollection<PlayerPoco>(_collection);

            var col = results.FindOneAsync(x => x.PlayerGuid.Equals(playerGUID));
            col.Result.XPosition = newPosX;
            col.Result.YPosition = newPosY;

           return await results.UpdateAsync(col.Result);
            
        }
   }
}