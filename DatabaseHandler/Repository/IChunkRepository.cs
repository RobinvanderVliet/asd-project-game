using System.Collections.Generic;
using System.Threading.Tasks;
using WorldGeneration.Models;

namespace DatabaseHandler.Repository
{
    public interface IChunkRepository
    {
        Task<string> CreateAsync(Chunk obj);
        Task<Chunk> ReadAsync(Chunk obj);
        Task<Chunk> UpdateAsync(Chunk obj);
        Task<int> DeleteAsync(Chunk obj);
        Task<IEnumerable<Chunk>> GetAllAsync();
        Task<int> DeleteAllAsync();
    }
}