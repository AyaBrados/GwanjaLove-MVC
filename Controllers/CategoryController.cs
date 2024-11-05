using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models;
using GwanjaLoveProto.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static GwanjaLoveProto.Data.Implementations.GlobalHelpers;

namespace GwanjaLoveProto.Controllers
{
    [Authorize(Roles = "Administrator, Developer")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork Uow; 
        private readonly UserManager<AppUser> UserManager;
        private AppUser? CurrentUser;

        public CategoryController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        public async Task<IActionResult> Index(BaseFilters? filters)
        {
            List<Category> values = new List<Category>();
            if (filters != null)
                values = await Uow.CategoryRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name == filters.Name) &&
                                                        (!filters.Active.HasValue || a.Active == filters.Active));
            else
                values = await Uow.CategoryRepository.GetAll();

            return View(new GenericLandingPageViewModel<Category> { Items = values, SuccessfullPersistence = filters?.SuccessfulPersistence, Filters = filters ?? new BaseFilters() });
        }

        public async Task<IActionResult> Category(int id)
        {
            try
            {
                return View(await Uow.CategoryRepository.FindAsync(id));
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
                await Uow.CategoryRepository.DeleteAsync(id);
                return RedirectToAction("Index", new BaseFilters { SuccessfulPersistence = Uow.Save() });
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
        public async Task<IActionResult> Add(Category category)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<Category>(ref category, true, CurrentUser?.UserName);
                await Uow.CategoryRepository.AddAsync(category);
                return RedirectToAction("Index", new BaseFilters { SuccessfulPersistence = Uow.Save() });
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
        public async Task<IActionResult> Update(Category category)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<Category>(ref category, category.Active, CurrentUser?.UserName);
                Uow.CategoryRepository.Update(category);
                return RedirectToAction("Index", new BaseFilters { SuccessfulPersistence = Uow.Save() });
            }
            catch
            {
                throw;
            }
        }

        private async Task GetCurrentUser()
        {
            string? userName = User.Identity?.Name;
            CurrentUser = await UserManager.FindByNameAsync(userName ?? "");
        }
    }
}
