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
            try
            {
                var result = await _db.GetCollection<T>(_collection).InsertAsync(obj);
                return result;
            }
            catch (LiteAsyncException ex)
            {
                Console.WriteLine("[{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.Source, ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("[InnerException][{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.InnerException.Source, ex.InnerException.Message);
                }
                Console.WriteLine(ex.StackTrace);
                throw new LiteAsyncException("Exception thrown in Repository.", ex);
            }
        }

        public async Task<int> UpdateAsync(T obj)
        {
            try {
                var results = await _db.GetCollection<T>(_collection).UpdateAsync(obj);
                return results ? 1 : 0;
            }
            catch (LiteAsyncException ex)
            {
                Console.WriteLine("[{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.Source, ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("[InnerException][{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.InnerException.Source, ex.InnerException.Message);
                }
                Console.WriteLine(ex.StackTrace);
                throw new LiteAsyncException("Exception thrown in Repository.", ex);
            }
        }

        public async Task<int> DeleteAsync(T obj)
        {
            try {
                var results = await _db.GetCollection<T>(_collection)
                    .DeleteManyAsync(c => c.Equals(obj));
                return results;
            }
            catch (LiteAsyncException ex)
            {
                Console.WriteLine("[{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.Source, ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("[InnerException][{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.InnerException.Source, ex.InnerException.Message);
                }
                Console.WriteLine(ex.StackTrace);
                throw new LiteAsyncException("Exception thrown in Repository.", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try {
                var result = await _db.GetCollection<T>(_collection).Query().ToListAsync();
                return result;
            }
            catch (LiteAsyncException ex)
            {
                Console.WriteLine("[{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.Source, ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("[InnerException][{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.InnerException.Source, ex.InnerException.Message);
                }
                Console.WriteLine(ex.StackTrace);
                throw new LiteAsyncException("Exception thrown in Repository.", ex);
            }
        }

        public async Task<int> DeleteAllAsync()
        {
            try {
                var result = await _db.GetCollection<T>(_collection).DeleteAllAsync();
                return result;
            }
            catch (LiteAsyncException ex)
            {
                Console.WriteLine("[{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.Source, ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("[InnerException][{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.InnerException.Source, ex.InnerException.Message);
                }
                Console.WriteLine(ex.StackTrace);
                throw new LiteAsyncException("Exception thrown in Repository.", ex);
            }
        }
    }
}