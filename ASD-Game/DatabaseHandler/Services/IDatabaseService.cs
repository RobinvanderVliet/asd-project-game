using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;

namespace ASD_Game.DatabaseHandler.Services
{
    public interface IDatabaseService<T>
    {
        Task<BsonValue> CreateAsync(T obj);
        Task<int> UpdateAsync(T obj);
        Task<int> DeleteAsync(T obj);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> DeleteAllAsync();
    }
}