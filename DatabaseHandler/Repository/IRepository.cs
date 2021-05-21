using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseHandler.Poco;
using LiteDB;

namespace DatabaseHandler.Repository
{
    public interface IRepository<T>
    {
        public Task<BsonValue> CreateAsync(T obj);
        public Task<T> ReadAsync(T obj);
        public Task<int> UpdateAsync(T obj);
        public Task<int> DeleteAsync(T obj);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<int> DeleteAllAsync();
        public Task<IEnumerable<PlayerPoco>> GetAllPoco();

    }
}