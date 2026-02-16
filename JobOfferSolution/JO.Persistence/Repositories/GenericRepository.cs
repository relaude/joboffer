using JO.Persistence.DataAccess;
using JO.Persistence.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace JO.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        //protected readonly IDbContextFactory<JobOfferDbContext> _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        //public GenericRepository(IDbContextFactory<JobOfferDbContext> context)
        //{
        //    _context = context;
        //    //_dbSet = _context.Set<T>();
        //}

        public async Task<T> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(params Expression<Func<T, bool>>[] predicates)
        {
            IQueryable<T> query = _dbSet;

            foreach (var predicate in predicates)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

        public void Update(T entity) => _dbSet.Update(entity);

        public void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<int> CountAllAsync() => await _dbSet.CountAsync();

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate) =>
            await _dbSet.FirstOrDefaultAsync(predicate);

        public async Task<T?> FirstOrDefaultAsync(params Expression<Func<T, bool>>[] predicates)
        {
            IQueryable<T> query = _dbSet;

            foreach (var predicate in predicates)
            {
                query = query.Where(predicate);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
