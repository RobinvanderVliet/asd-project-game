using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using ASD_Game.DatabaseHandler.POCO;
using LiteDB;
using LiteDB.Async;

namespace ASD_Game.DatabaseHandler.Repository
{
    [ExcludeFromCodeCoverage]
    public class Repository<T> : IRepository<T>
    {
        private readonly string _collection;
        private readonly ILiteDatabaseAsync _db;

        [ExcludeFromCodeCoverage]
        public Repository(string collection = null)
        {
            IDbConnection connection = new DbConnection();
            _db = connection.GetConnectionAsync();
            _collection = collection ?? typeof(T).Name;
        }

        public async Task<BsonValue> CreateAsync(T obj)
        {
            var result = await _db.GetCollection<T>(_collection).InsertAsync(obj);
            return result;
        }

        public async Task<int> UpdateAsync(T obj)
        {
            var results = await _db.GetCollection<T>(_collection).UpdateAsync(obj);
                return results ? 1 : 0;
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
    }
}