using System.Linq.Expressions;

namespace GwanjaLoveProto.Data.Interfaces
{
	public interface IRepositoryBase<T>
	{
		Task<T?> FindAsync(int id);
		Task<List<T>> GetAll();
		Task<List<T>> GetFilteredCollectionAsync(Expression<Func<T, bool>> expression);
		Task AddAsync(T entity);
		void Update(T entity);
		Task DeleteAsync(int id);
		Task AddRangeAsync(IEnumerable<T> entities);
		void UpdateRange(IEnumerable<T> entities);
		Task DeleteRangeAsync(IEnumerable<int> ids);
		Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
		Task<bool> AllAsync(Expression<Func<T, bool>> expression);
		Task<int> CountAsync();
		Task<decimal> Sum(Expression<Func<T, decimal>> expression);
		Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
	}
}
