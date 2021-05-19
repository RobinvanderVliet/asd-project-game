using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHandler.Poco;
using DatabaseHandler.Repository;
using LiteDB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DatabaseHandler.Services
{
    public class ServicesDb<T> : IServicesDb<T>
    {
        private readonly ILogger<ServicesDb<T>> _log;
        private readonly IRepository<T> _repository;

        public ServicesDb(IRepository<T> repository)
        {
            
            _repository = repository;
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

        public Task<T> ReadAsync(T obj)
        {
            try
            {
                return _repository.ReadAsync(obj);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception thrown trying to read a {Obj} from database: {Message}", typeof(T), ex.Message);
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