using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models.ViewModels;
using GwanjaLoveProto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static GwanjaLoveProto.Data.Implementations.GlobalHelpers;

namespace GwanjaLoveProto.Controllers
{
    [Authorize(Roles = "Administrator, Developer")]
    public class ShopSpecialController : Controller
    {
        private readonly IUnitOfWork Uow;
        private readonly UserManager<AppUser> UserManager;
        private AppUser? CurrentUser;

        public ShopSpecialController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(BaseFilters? filters)
        {
            List<ShopSpecial> values = new List<ShopSpecial>();
            if (filters != null)
            {
                values = await Uow.ShopSpecialRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name.Contains(filters.Name)));
            }
            else
            {
                values = await Uow.ShopSpecialRepository.GetAll();
            }

            return View(new GenericLandingPageViewModel<ShopSpecial> { Items = values, SuccessfullPersistence = filters?.SuccessfulPersistence, Filters = filters ?? new BaseFilters() });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Special(int? id)
        {
            try
            {
                if (id.HasValue)
                    return View(await Uow.ShopSpecialRepository.FindAsync(id.Value));
                else
                    return View(null);
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
                await Uow.ShopSpecialRepository.DeleteAsync(id);
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
        public async Task<IActionResult> Add(ShopSpecial special)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<ShopSpecial>(ref special, true, CurrentUser.UserName);
                await Uow.ShopSpecialRepository.AddAsync(special);
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
        public async Task<IActionResult> Update(ShopSpecial special)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<ShopSpecial>(ref special, special.Active, CurrentUser.UserName);
                Uow.ShopSpecialRepository.Update(special);
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
