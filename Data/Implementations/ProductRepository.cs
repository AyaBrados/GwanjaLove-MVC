using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Data.Exceptions;
using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models;

namespace GwanjaLoveProto.Data.Implementations
{
	public class ProductRepository : IRepositoryBase<Product>
	{
		private readonly AppDBContext _appDbContext;

		public ProductRepository(AppDBContext appDbContext)
		{
			_appDbContext = appDbContext;
			RefreshMemory();
		}

		public List<Product> rawValues = new List<Product>();
		public bool Delete(int id)
		{
			var record = FindById(id);

			_appDbContext.Products.Remove(record);
			return Save();
		}

		public Product? FindById(int id)
		{
			RefreshMemory();
			var product = rawValues.FirstOrDefault(x => x.Id == id);

			if (product == null)
				throw new EntityNotFoundException($"Product with Id: {id} cannot be found.");

			return product;
		}

		public List<Product> GetAll()
		{
			RefreshMemory();
			return rawValues;
		}

		public List<Product> GetFilteredCollection(object filters)
		{
			RefreshMemory();
			var filter = (ProductLandingFilters)filters;
			return rawValues.AsQueryable()
				.Where(x => string.IsNullOrEmpty(filter.Name) || x.Name.Equals(filter.Name))
				.Where(x => !filter.CategoryId.HasValue || x.CategoryId == filter.CategoryId)
				.Where(x => !filter.IsInStock.HasValue || x.IsInStock == filter.IsInStock)
				.ToList();
		}

		public bool Add(Product entity)
		{
			IsEntityDuplicate(entity);
			_appDbContext.Products.Add(entity);

			return Save();
		}

		public bool Update(Product entity)
		{
			FindById(entity.Id);
			IsEntityDuplicate(entity);
			_appDbContext.Products.Update(entity);

			return Save();
		}

        public bool IsEntityDuplicate(Product entity)
        {
			RefreshMemory();
            var exist = rawValues.Any(x => x.Name == entity.Name
								&& x.Category.Name == entity.Category.Name && x.Id != entity.Id);

			if (exist)
				throw new EntityDuplicationException($"Product with name: {entity.Name} in category: {entity.Category.Name} already exist.");

			return exist;
        }

		private void RefreshMemory()
		{
			rawValues = _appDbContext.Products.ToList();
		}

		private bool Save()
		{
            return _appDbContext.SaveChanges() > -1;
        }
    }
}
