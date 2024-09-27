using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models;
using System;

namespace GwanjaLoveProto.Data.Implementations
{
	public class ProductRepository : IRepositoryBase<Product>
	{
		private readonly AppDBContext _appDbContext;

		public ProductRepository(AppDBContext appDbContext)
		{
			_appDbContext = appDbContext;
			rawValues = _appDbContext.Products.ToList();
		}

		public List<Product> rawValues = new List<Product>();
		public bool Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Product? FindById(int id)
		{
			return rawValues.FirstOrDefault(x => x.Id == id);
		}

		public List<Product> GetAll()
		{
			return rawValues;
		}

		public List<Product> GetFilteredCollection(object filters)
		{
			var filter = (ProductLandingFilters)filters;
			return rawValues.AsQueryable()
				.Where(x => string.IsNullOrEmpty(filter.Name) || x.Name.Equals(filter.Name))
				.Where(x => !filter.CategoryId.HasValue || x.CategoryId == filter.CategoryId)
				.Where(x => !filter.IsInStock.HasValue || x.IsInStock == filter.IsInStock)
				.ToList();
		}

		public bool Save(Product entity)
		{
			throw new NotImplementedException();
		}

		public bool Update(Product entity)
		{
			throw new NotImplementedException();
		}
	}
}
