using DatabaseHandler.Repository;
using LiteDB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseHandler.Services
{
    public class ServicesDb<T> : IServicesDb<T>
    {
        private readonly ILogger<ServicesDb<T>> _log;
        private readonly IRepository<T> _repository;

        public ServicesDb(IRepository<T> repository = null)
        {
            _repository = repository ?? new Repository<T>();
            _log = new NullLogger<ServicesDb<T>>();
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