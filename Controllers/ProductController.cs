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
        public async Task<IActionResult> Index(ProductLandingFilters? filters)
        {
            List<Product> values = new List<Product>();
            if (filters != null)
                values = await Uow.ProductRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name == filters.Name) &&
                                                        (filters.Category == null || a.CategoryId == filters.Category.Id) &&
                                                        (!filters.IsInStock.HasValue || a.IsInStock == filters.IsInStock) &&
                                                        (!filters.Active.HasValue || a.Active == filters.Active));
            else
                values = await Uow.ProductRepository.GetAll();

            await PopulateCategories(filters?.Category);
            return View(new GenericLandingPageViewModel<Product> { Items = values, SuccessfullPersistence = filters?.SuccessfulPersistence, Filters = filters ?? new ProductLandingFilters() });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Product(int id)
        {
            try
            {
                return View(await Uow.ProductRepository.FindAsync(id));
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
                await Uow.ProductRepository.DeleteAsync(id);
                return RedirectToAction("Index", Uow.Save());
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
                return RedirectToAction("Index", Uow.Save());
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
                return RedirectToAction("Index", Uow.Save());
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
        }

        private async Task GetCurrentUser()
        {
            string? userName = User.Identity?.Name;
            CurrentUser = await UserManager.FindByNameAsync(userName ?? "");
        }
    }
}
