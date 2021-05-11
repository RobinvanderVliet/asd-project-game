using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseHandler.Repository;
using LiteDB;
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

        public async Task<string> CreateAsync(Chunk obj)
        {
            try
            {
                return _repository.Create(obj);
            }
            catch (LiteException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Chunk Read(Chunk obj)
        {
            try
            {
                return _repository.Read(obj);
            }
            catch (LiteException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Chunk> UpdateAsync(Chunk obj)
        {
            try
            {
                return await _repository.UpdateAsync(obj);
            }
            catch (LiteException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<int> DeleteAsync(Chunk obj)
        {
            try
            {
                return _repository.Delete(obj);
            }
            catch (LiteException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public IEnumerable<Chunk> GetAll()
        {
            try
            {
                return _repository.GetAll();
            }
            catch (LiteException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public int DeleteAll()
        {
            try
            {
                return _repository.DeleteAll();
            }
            catch (LiteException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}