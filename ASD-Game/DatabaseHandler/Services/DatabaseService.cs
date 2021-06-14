using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASD_Game.DatabaseHandler.Repository;
using LiteDB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ASD_Game.DatabaseHandler.Services
{
    public class DatabaseService<T> : IDatabaseService<T>
    {
        private readonly ILogger<DatabaseService<T>> _log;
        private readonly IRepository<T> _repository;

        public DatabaseService(IRepository<T> repository = null)
        {
            _repository = repository ?? new Repository<T>();
            _log = new NullLogger<DatabaseService<T>>();
        }

        public Task<BsonValue> CreateAsync(T obj)
        {
            try
            {
                return _repository.CreateAsync(obj);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception thrown trying to create a {Obj}: {Message}", typeof(T), ex.Message);
                throw;
            }
        }

        public Task<int> UpdateAsync(T obj)
        {
            try
            {
                return _repository.UpdateAsync(obj);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception thrown trying to update a {Obj}: {Message}", typeof(T), ex.Message);
                throw;
            }
        }
        
        public Task<int> DeleteAsync(T obj)
        {
            try
            {
                return _repository.DeleteAsync(obj);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception thrown trying to delete {Obj} from database: {Message}", typeof(T), ex.Message);
                throw;
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