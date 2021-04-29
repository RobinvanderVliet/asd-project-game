using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseHandler
{
    public interface IRepository<T>
    {
        Task<T> CreateASync(T obj);
        Task<T> ReadASync(T obj);
        Task<T> UpdateASync(T oldObj, T newObj);
        Task<T> DeleteASync(T obj);
        Task<IList<T>> GetAllASync();
        Task<int> DeleteAllASync();
    }
}