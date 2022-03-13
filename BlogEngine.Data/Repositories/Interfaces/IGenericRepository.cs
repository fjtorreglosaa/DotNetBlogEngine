using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogEngine.Data.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T:class
    {
        IQueryable<T> GetAll();
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Delete(T entity);
    }
}
