using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseHandler.POCO;
using LiteDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseHandler.Repository
{
    public interface IRepository<T>
    {
        Task<BsonValue> CreateAsync(T obj);
        Task<int> UpdateAsync(T obj);
        Task<int> DeleteAsync(T obj);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> DeleteAllAsync();

    }
}