using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models;

namespace GwanjaLoveProto.Data.Implementations
{
	public class ProductRepositoryBase : IRepositoryBase<Product>
	{
		public IEnumerable<Product> rawValues = new List<Product>();
		public bool Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Product? FindById(int id)
		{
			return rawValues.FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<Product> GetAll()
		{
			return rawValues;
		}

		public IEnumerable<Product> GetFilteredCollection(string? name, string? productCategory, bool? isInStock)
		{
			return rawValues.AsQueryable()
				.Where(x => string.IsNullOrEmpty(name) || x.Name.Equals(name))
				.Where(x => string.IsNullOrEmpty(productCategory) || x.Category.Name.Equals(productCategory))
				.Where(x => !isInStock || x => x.IsInStock == isInStock);
		}

		public bool Save(T entity)
		{
			throw new NotImplementedException();
		}

		public bool Update(T entity)
		{
			throw new NotImplementedException();
		}
	}
}
