using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;
using WorldGeneration.Models;

namespace DatabaseHandler.Repository
{
    public interface IChunkRepository
    {
        Task<BsonValue> CreateAsync(Chunk obj);
        Task<Chunk> ReadAsync(Chunk obj);
        Task<Chunk> UpdateAsync(Chunk oldObj, Chunk newObj);
        Task<Chunk> DeleteAsync(Chunk obj);
        Task<IList<Chunk>> GetAllAsync();
        Task<int> DeleteAllAsync();
    }
}