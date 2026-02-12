using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace JO.Persistence.Repositories.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<int> SaveChangesAsync();
        Task<IEnumerable<T>> FindAsync(params Expression<Func<T, bool>>[] predicates);
        Task<int> CountAllAsync();
        void UpdateRange(IEnumerable<T> entities);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(params Expression<Func<T, bool>>[] predicates);
    }
}
