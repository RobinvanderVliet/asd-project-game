using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using LiteDB;
using LiteDB.Async;

namespace DatabaseHandler.Repository
{
    [ExcludeFromCodeCoverage]
    public class Repository<T> : IRepository<T>
    {
        private readonly string _collection;
        private readonly IDbConnection _connection;
        private readonly ILiteDatabaseAsync _db;

        [ExcludeFromCodeCoverage]
        public Repository(string connectionString = null, string collection = null)
        {
            /*
             * TODO: Connection string zo aanpassen dat je alleen filename meegeeft.
             */
            _collection = collection ?? typeof(T).Name;
            _connection = new DbConnection();
            _db = _connection.GetConnectionAsync();
            _connection.SetConnectionString(connectionString ?? "Filename=.\\" + typeof(T).Name + ".db");
        }

        [ExcludeFromCodeCoverage]
        public async Task<BsonValue> CreateAsync(T obj)
        {
            var result = await _db.GetCollection<T>(_collection).InsertAsync(obj).ConfigureAwait(false);
            return result;
        }

        [ExcludeFromCodeCoverage]
        public async Task<T> ReadAsync(T obj)
        {
            var chunk = await _db.GetCollection<T>(_collection)
                .FindOneAsync(c => c.Equals(obj)).ConfigureAwait(false);
            return chunk;
        }

        [ExcludeFromCodeCoverage]
        public async Task<int> UpdateAsync(T obj)
        {
            var results = await _db.GetCollection<T>(_collection).UpdateAsync(obj).ConfigureAwait(false);
            return results ? 1 : throw new InvalidOperationException($"Object op type {typeof(T)} does not exist in database.");
        }

        [ExcludeFromCodeCoverage]
        public async Task<int> DeleteAsync(T obj)
        {
            var results = await _db.GetCollection<T>(_collection)
                .DeleteManyAsync(c => c.Equals(obj)).ConfigureAwait(false);
            return results;
        }

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var chunks = await _db.GetCollection<T>(_collection).Query().ToListAsync().ConfigureAwait(false);
            return chunks;
        }

        [ExcludeFromCodeCoverage]
        public async Task<int> DeleteAllAsync()
        {
            var result = await _db.GetCollection<T>(_collection).DeleteAllAsync().ConfigureAwait(false);
            return result;
        }
    }
}