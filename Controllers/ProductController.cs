using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models;
using GwanjaLoveProto.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static GwanjaLoveProto.Data.Implementations.GlobalHelpers;

namespace GwanjaLoveProto.Controllers
{
    [Authorize(Roles = "Administrator, Developer")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork Uow;
        private readonly UserManager<AppUser> UserManager;
        private AppUser? CurrentUser;

        public ProductController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(ProductLandingFilters filters)
        {
            List<Product> values = new List<Product>();
            List<ProductViewModel> returnValues = new List<ProductViewModel>();
            if (filters != null)
            {
                var sorting = GetSortingValue(filters.SortingAndPagingFilters?.SortBy); 
                values = await Uow.ProductRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name == filters.Name) &&
                                                        (filters.Category == null || a.CategoryId == filters.Category.Id) &&
                                                        ((string.IsNullOrEmpty(sorting) && !sorting.Equals("IsInStock")) || a.IsInStock == filters.IsInStock) &&
                                                        (!filters.Active.HasValue || a.Active == filters.Active));
            }
            else
                values = await Uow.ProductRepository.GetAll();

            foreach (Product p in values)
            {
                returnValues.Add(new ProductViewModel
                {
                    Product = p,
                    Favourites = await Uow.UserFavouriteRepository.GetFilteredCollectionAsync(x => x.ProductId == p.Id)
                });
            }

            await PopulateCategories(filters?.Category);
            return View(new GenericLandingPageViewModel<ProductViewModel> { Items = returnValues, SuccessfullPersistence = filters.SuccessfullPersistence, Filters = filters ?? new ProductLandingFilters() });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Product(int id)
        {
            try
            {
                var product = await Uow.ProductRepository.FindAsync(id);
                await GetRelatedProducts(product.CategoryId);
                
				return product != null ? View(new ProductViewModel {
                    Product = product,
                    Reviews = await Uow.SurveyResponseRepository.GetFilteredCollectionAsync(x => x.Approved && x.ProductId == product.Id),
                    Favourites = await Uow.UserFavouriteRepository.GetFilteredCollectionAsync(x => x.ProductId == product.Id)
				}) : throw new Exception("Product was not found, please reload the Product home page.");
            }
            catch
            {
                throw;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await Uow.ProductRepository.FindAsync(id);
                await Uow.ProductRepository.DeleteAsync(id);
                return RedirectToAction("Index", new ProductLandingFilters
				{
					SuccessfullPersistence = new SuccessfullPersistenceViewModel
					{
						SuccessfulPersistence = Uow.Save(),
						EntityName = $"Product: {product?.Name} successfully deleted."
					}
				});
            }
            catch
            {
                throw;
            }
        }

        public IActionResult Add()
        {
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<Product>(ref product, true, CurrentUser.UserName);
                await Uow.ProductRepository.AddAsync(product);
                return RedirectToAction("Index", new ProductLandingFilters
                {
                    SuccessfullPersistence = new SuccessfullPersistenceViewModel
                    {
                        SuccessfulPersistence = Uow.Save(),
                        EntityName = $"Product: {product.Name} successfully added."
                    }
                });
            }
            catch
            {
                throw;
            }
        }

        public IActionResult Update()
        {
            return View();
        }
        
        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<Product>(ref product, product.Active, CurrentUser.UserName);
                Uow.ProductRepository.Update(product);
                return RedirectToAction("Index", new ProductLandingFilters 
                { 
                    SuccessfullPersistence = new SuccessfullPersistenceViewModel 
                    { 
                        SuccessfulPersistence = Uow.Save(),
                        EntityName = $"Product: {product.Name} successfully updated." 
                    }
                });
            }
            catch
            {
                throw;
            }
        }

        private async Task PopulateCategories(Category? category)
        {
            var selectedCategory = await Uow.CategoryRepository.FindAsync(category != null ? category.Id : 0);
            var allCategories = Uow.CategoryRepository.GetAll();
            ViewBag.Categories = selectedCategory != null && category != null ? new MultiSelectList((System.Collections.IEnumerable)allCategories, new List<Category> { selectedCategory })
                                    : new MultiSelectList((System.Collections.IEnumerable)allCategories);
            ViewBag.Sorting = new SelectList(GetSortingEnums());
            ViewBag.SortDirection = new SelectList(GetDirectionsEnum());
        }

        private async Task GetRelatedProducts(int categoryId)
        {
            var relatedCategory = await Uow.ProductRepository.GetFilteredCollectionAsync(x => x.CategoryId == categoryId);

            ViewBag.RelatedProducts = relatedCategory;
        }

        private async Task GetCurrentUser()
        {
            string? userName = User.Identity?.Name;
            CurrentUser = await UserManager.FindByNameAsync(userName ?? "");
        }
    }
}
