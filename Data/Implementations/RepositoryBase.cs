using GwanjaLoveProto.Data.Exceptions;
using GwanjaLoveProto.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace GwanjaLoveProto.Data.Implementations
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly AppDBContext _dbContext;

        public RepositoryBase(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await _dbContext.Set<T>().AddAsync(entity);
            }
        }

        public async Task<bool> AllAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().AllAsync(expression);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().AnyAsync(expression);
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var record = await FindAsync(id);
            if (record != null)
                _dbContext.Set<T>().Remove(record);
        }

        public async Task DeleteRangeAsync(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                var record = await FindAsync(id);
                if (record != null)
                    _dbContext.Set<T>().Remove(record);
            }
        }

        public async Task<T?> FindAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetFilteredCollectionAsync(Expression<Func<T, bool>> filters)
        {
            return await _dbContext.Set<T>().Where(filters).ToListAsync();
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _dbContext.Set<T>().Update(entity);
            }
        }

        public async Task<decimal> Sum(Expression<Func<T, decimal>> filters)
        {
            return await _dbContext.Set<T>().SumAsync(filters);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filters)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(filters);
        }
    }
}
