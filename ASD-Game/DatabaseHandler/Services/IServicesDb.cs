using LiteDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseHandler.Services
{
    public interface IServicesDb<T>
    {
        public Task<BsonValue> CreateAsync(T obj);
        public Task<T> ReadAsync(T obj);
        public Task<int> UpdateAsync(T obj);
        public Task<int> DeleteAsync(T obj);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<int> DeleteAllAsync();
    }
}