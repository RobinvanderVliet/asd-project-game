using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ASD_Game.DatabaseHandler.Repository;
using LiteDB;
using LiteDB.Async;

namespace ASD_Game.DatabaseHandler.Services
{
    public class DatabaseService<T> : IDatabaseService<T>
    {
        private readonly IRepository<T> _repository;

        public DatabaseService(IRepository<T> repository = null)
        {
            _repository = repository ?? new Repository<T>();
        }

        public Task<BsonValue> CreateAsync(T obj)
        {
            try
            {
                return _repository.CreateAsync(obj);
            }
            catch (LiteAsyncException ex)
            {
                Console.WriteLine("[{0}][{1}] ({2}) Source: {3}, Message: {4}\r\n[StackTrace] {5}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.Source, ex.Message, ex.StackTrace);
                throw new LiteAsyncException("Exception thrown in DatabaseService.", ex);
            }
        }

        public Task<int> UpdateAsync(T obj)
        {
            try
            {
                return _repository.UpdateAsync(obj);
            }
            catch (LiteAsyncException ex)
            {
                Console.WriteLine("[{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.Source, ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("[InnerException][{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.InnerException.Source, ex.InnerException.Message);
                }
                Console.WriteLine(ex.StackTrace);
                throw new LiteAsyncException("Exception thrown in DatabaseService.", ex);
            }
        }
        
        public Task<int> DeleteAsync(T obj)
        {
            try
            {
                return _repository.DeleteAsync(obj);
            }
            catch (LiteAsyncException ex)
            {
                Console.WriteLine("[{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.Source, ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("[InnerException][{0}][{1}] ({2}) Source: {3}, Message: {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, typeof(T), ex.InnerException.Source, ex.InnerException.Message);
                }
                Console.WriteLine(ex.StackTrace);
                throw new LiteAsyncException("Exception thrown in DatabaseService.", ex);
            }
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<int> DeleteAllAsync()
        {
            return _repository.DeleteAllAsync();
        }
    }
}