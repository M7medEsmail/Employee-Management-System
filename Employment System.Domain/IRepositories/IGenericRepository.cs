using Employment_System.Domain.ISpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Domain.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        bool UpdateAsync(int id,T entity);
        Task<bool> DeleteAsync(int id);  
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<T> GetByIdWithSpecAsync(ISpecification<T> spec);
    }
}
