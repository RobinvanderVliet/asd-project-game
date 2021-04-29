using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseHandler.Repository;
using LiteDB;
using Microsoft.Extensions.Logging;
using WorldGeneration.Models;

namespace DatabaseHandler.Services
{
    public class ChunkServices : IChunkServices
    {
        private readonly ILogger<ChunkServices> _log;
        private readonly IChunkRepository _repository;
        
        public ChunkServices(ILogger<ChunkServices> log, IChunkRepository repository)
        {
            _log = log;
            _repository = repository;
        }
        
        public async Task<BsonValue> CreateAsync(Chunk obj)
        {
            try
            {
                return await _repository.CreateAsync(obj);
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Chunk> ReadAsync(Chunk obj)
        {
            try
            {
                return await _repository.ReadAsync(obj);
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Chunk> UpdateAsync(Chunk oldObj, Chunk newObj)
        {
            try
            {
                return await _repository.UpdateAsync(oldObj, newObj);
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Chunk> DeleteAsync(Chunk obj)
        {
            try
            {
                return await _repository.DeleteAsync(obj);
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IList<Chunk>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<int> DeleteAllAsync()
        {
            try
            {
                return await _repository.DeleteAllAsync();
            }
            catch (LiteException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }
    }
}