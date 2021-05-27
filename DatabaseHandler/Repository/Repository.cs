using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
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
        public Repository(string collection = null)
        {
            IDbConnection connection = new DbConnection();
            _db = connection.GetConnectionAsync();
            _collection = collection ?? typeof(T).Name;
            _log = new NullLogger<Repository<T>>();
        }

        [ExcludeFromCodeCoverage]
        public async Task<BsonValue> CreateAsync(T obj)
        {
            var result = await _db.GetCollection<T>(_collection).InsertAsync(obj);
            return result;
        }

        [ExcludeFromCodeCoverage]
        public async Task<int> UpdateAsync(T obj)
        {
            var results = await _db.GetCollection<T>(_collection).UpdateAsync(obj);
            
            if (results)
            {
                return 1;
            }
            throw new InvalidOperationException($"Object op type {typeof(T)} does not exist in database.");
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
            var chunks = await _db.GetCollection<T>(_collection).Query().ToListAsync();
            return chunks;
        }

        [ExcludeFromCodeCoverage]
        public async Task<int> DeleteAllAsync()
        {
            var result = await _db.GetCollection<T>(_collection).DeleteAllAsync();
            return result;
        }
    }
}