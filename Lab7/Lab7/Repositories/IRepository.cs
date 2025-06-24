using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab7.Repositories
{
    /// <summary>
    /// Общий CRUD-интерфейс.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
}
