using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DatabaseHandler.POCO;
using LiteDB;
using LiteDB.Async;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DatabaseHandler.Repository
{
    public class Repository<T> : IRepository<T>
    {
        private readonly string _collection;
        private readonly ILiteDatabaseAsync _db;
        private readonly ILogger<Repository<T>> _log;

        public Repository(IDbConnection connection, string collection = null)
        {
            _collection = collection ?? typeof(T).Name;
            _db = connection.GetConnectionAsync();
            _log = new NullLogger<Repository<T>>();
        }

        public async Task<BsonValue> CreateAsync(T obj)
        {
            var result = await _db.GetCollection<T>(_collection).InsertAsync(obj);
            return result;
        }

        public async Task<T> ReadAsync(T obj)
        {
            var chunk = await _db.GetCollection<T>(_collection)
                .FindOneAsync(c => c.Equals(obj));
            return chunk;
        }

        public async Task<int> UpdateAsync(T obj)
        {
            var results = await _db.GetCollection<T>(_collection).UpdateAsync(obj);
            
            if (results)
            {
                return 1;
            }
            throw new InvalidOperationException($"Object op type {typeof(T)} does not exist in database.");
        }

        public async Task<int> DeleteAsync(T obj)
        {
            var results = await _db.GetCollection<T>(_collection)
                .DeleteManyAsync(c => c.Equals(obj));
            return results;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await _db.GetCollection<T>(_collection).Query().ToListAsync();
            return result;
        }

        public async Task<int> DeleteAllAsync()
        {
            var result = await _db.GetCollection<T>(_collection).DeleteAllAsync();
            return result;
        }

        public async Task<IEnumerable<PlayerPOCO>> GetAllPOCO()
        {
            var result = await _db.GetCollection<PlayerPOCO>(_collection).Query().ToListAsync();
            return result; 
        }
        
        public async Task<Boolean> UpdateAsyncPlayer(string playerGUID, int newPosX, int newPosY)
        {
            var results =  _db.GetCollection<PlayerPOCO>(_collection);

            var col = results.FindOneAsync(x => x.PlayerGuid.Equals(playerGUID));
            col.Result.XPosition = newPosX;
            col.Result.YPosition = newPosY;

           return await results.UpdateAsync(col.Result);
        }
   }
}