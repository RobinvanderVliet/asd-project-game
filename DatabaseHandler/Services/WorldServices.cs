using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Logging;
using WorldGeneration.Models;

namespace DatabaseHandler.Services
{
    public class WorldServices : IServices<Chunk>
    {
        private readonly ILogger<WorldServices> _log;
        private readonly IRepository<Chunk> _repository;
        
        public WorldServices(ILogger<WorldServices> log, IRepository<Chunk> repository)
        {
            _log = log;
            _repository = repository;
        }
        
        public async Task<Chunk> CreateASync(Chunk obj)
        {
            try
            {
                return await _repository.CreateASync(obj);
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Chunk> ReadASync(Chunk obj)
        {
            try
            {
                return await _repository.ReadASync(obj);
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Chunk> UpdateASync(Chunk oldObj, Chunk newObj)
        {
            try
            {
                return await _repository.UpdateASync(oldObj, newObj);
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Chunk> DeleteASync(Chunk obj)
        {
            try
            {
                return await _repository.DeleteASync(obj);
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IList<Chunk>> GetAllASync()
        {
            try
            {
                return await _repository.GetAllASync();
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<int> DeleteAllASync()
        {
            try
            {
                return await _repository.DeleteAllASync();
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }
    }
}