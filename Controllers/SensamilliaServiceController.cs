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
    public class SensamilliaServiceController : Controller
    {
        private readonly IUnitOfWork Uow; 
        private readonly UserManager<AppUser> UserManager;
        private AppUser? CurrentUser;

        public SensamilliaServiceController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(BaseFilters? filters)
        {
            List<SensamilliaService> values = new List<SensamilliaService>();
            if (filters != null)
            {
                values = await Uow.SensamilliaServiceRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name.Contains(filters.Name)));
            }
            else
            {
                values = await Uow.SensamilliaServiceRepository.GetAll();
            }

            return View(new GenericLandingPageViewModel<SensamilliaService> { Items = values, SuccessfullPersistence = filters?.SuccessfulPersistence, Filters = filters ?? new BaseFilters() });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Service(int? id)
        {
            try
            {
                if (id.HasValue)
                    return View(await Uow.SensamilliaServiceRepository.FindAsync(id.Value));
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
                await Uow.SensamilliaServiceRepository.DeleteAsync(id);
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
        public async Task<IActionResult> Add(SensamilliaService service)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<SensamilliaService>(ref service, true, CurrentUser.UserName);
                await Uow.SensamilliaServiceRepository.AddAsync(service);
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
        public async Task<IActionResult> Update(SensamilliaService service)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<SensamilliaService>(ref service, service.Active, CurrentUser.UserName);
                Uow.SensamilliaServiceRepository.Update(service);
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
