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
        
        public Chunk Create(Chunk obj)
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

        public Chunk Update(Chunk oldObj, Chunk newObj)
        {
            try
            {
                return _repository.Update(oldObj, newObj);
            }
            catch (LiteException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Chunk Delete(Chunk obj)
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