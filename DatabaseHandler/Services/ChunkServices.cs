using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseHandler.Repository;
using WorldGeneration.Models;

namespace DatabaseHandler.Services
{
    public class ChunkServices : IChunkServices
    {
        private readonly IChunkRepository _repository;
        
        public ChunkServices( IChunkRepository repository)
        {
            _repository = repository;
        }

        public Task<string> CreateAsync(Chunk obj)
        {
            try
            {
                return _repository.CreateAsync(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
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