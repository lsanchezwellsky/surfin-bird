using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<List<T>> GetAllRecords();

        Task<T> GetAsync(int id);

        IQueryable<T> Query();

        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int[] ids);

        Task<IEnumerable<IDictionary<string, object>>> ExecuteQuery(string query, object[] parameters);
    }
}