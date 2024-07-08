namespace GwanjaLoveProto.Data.Interfaces
{
	public interface IRepositoryBase<T>
	{
		T? FindById(int id);
		IEnumerable<T> GetAll();
		IEnumerable<T> GetFilteredCollection(string name, string productCategory, bool? isInStock);
		bool Save(T entity);
		bool Update(T entity);
		bool Delete(int id);
	}
}
