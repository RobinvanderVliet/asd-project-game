using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseHandler.Repository;
using Microsoft.Extensions.Logging;
using WorldGeneration.Models;

namespace DatabaseHandler.Services
{
    public class ChunkServices : IChunkServices
    {
        private readonly ILogger<ChunkServices> _log;
        private readonly IChunkRepository _repository;

        public ChunkServices(IChunkRepository repository, ILogger<ChunkServices> log)
        {
            _repository = repository;
            _log = log;
        }

        public Task<string> CreateAsync(Chunk obj)
        {
            try
            {
                return _repository.CreateAsync(obj);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {Message}", ex.Message);
                throw;
            }
        }

        public Task<Chunk> ReadAsync(Chunk obj)
        {
            try
            {
                return _repository.ReadAsync(obj);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {Message}", ex.Message);
                throw;
            }
        }

        public Task<Chunk> UpdateAsync(Chunk obj)
        {
            try
            {
                return _repository.UpdateAsync(obj);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {Message}", ex.Message);
                throw;
            }
        }

        public Task<int> DeleteAsync(Chunk obj)
        {
            try
            {
                return _repository.DeleteAsync(obj);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {Message}", ex.Message);
                throw;
            }
        }

        public Task<IEnumerable<Chunk>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<int> DeleteAllAsync()
        {
            return _repository.DeleteAllAsync();
        }
    }
}