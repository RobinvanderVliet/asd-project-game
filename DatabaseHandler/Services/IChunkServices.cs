using System.Collections.Generic;
using WorldGeneration.Models;

namespace DatabaseHandler.Services
{
    public interface IChunkServices
    {
        Chunk Create(Chunk obj);
        Chunk Read(Chunk obj);
        Chunk Update(Chunk oldObj, Chunk newObj);
        Chunk Delete(Chunk obj);
        IEnumerable<Chunk> GetAll();
        int DeleteAll();
    }
}