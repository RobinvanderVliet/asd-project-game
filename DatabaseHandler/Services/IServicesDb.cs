using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;

namespace DatabaseHandler.Services
{
    public interface IServicesDb<T>
    {
        Task<BsonValue> CreateAsync(T obj);
        Task<T> ReadAsync(T obj);
        Task<int> UpdateAsync(T obj);
        Task<int> DeleteAsync(T obj);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> DeleteAllAsync();
    }
}