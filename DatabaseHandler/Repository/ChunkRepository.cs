using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Logging;
using WorldGeneration.Models;

namespace DatabaseHandler.Repository
{
    public class ChunkRepository : IChunkRepository
    {
        private readonly IDbConnection _connection;
        private readonly string _collection;

        public ChunkRepository(IDbConnection connection, string collection = "Chunks")
        {
            _connection = connection;
            _collection = collection;
        }
        
        public Chunk Create(Chunk obj)
        {
            var db = _connection.GetConnection();
            var result = db.GetCollection<Chunk>(_collection).Insert(obj);
            return obj;
        }

        public Chunk Read(Chunk obj)
        {
            var db = _connection.GetConnection();
            var chunk =  db.GetCollection<Chunk>(_collection).FindOne(chunk => chunk.X.Equals(obj.X) && chunk.Y.Equals(obj.Y));
            return chunk;
        }

        public Chunk Update(Chunk oldObj, Chunk newObj)
        {
            //using var db = _connection.getConnectionASync();
            //return await db.GetCollection<Chunk>(_collection).UpdateAsync();
            throw new System.NotImplementedException();
        }

        public Chunk Delete(Chunk obj)
        {
            //using var db = _connection.getConnectionASync();
            //return await db.GetCollection<Chunk>(_collection).DeleteAsync();
            throw new System.NotImplementedException();
        }

        public IEnumerable<Chunk> GetAll()
        {
            var db = _connection.GetConnection();
            var chunks = db.GetCollection<Chunk>(_collection).Query().ToList();
            return chunks;
        }

        public int DeleteAll()
        {
            var db = _connection.GetConnection();
            var result = db.GetCollection<Chunk>(_collection).DeleteAll();
            return result;
        }
    }
}