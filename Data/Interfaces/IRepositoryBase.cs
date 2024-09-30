namespace GwanjaLoveProto.Data.Interfaces
{
	public interface IRepositoryBase<T>
	{
		T? FindById(int id);
		List<T> GetAll();
		List<T> GetFilteredCollection(object filters);
		bool IsEntityDuplicate(T entity);
		private void RefreshMemory() { }
		bool Add(T entity);
		bool Update(T entity);
		bool Delete(int id);
	}
}
