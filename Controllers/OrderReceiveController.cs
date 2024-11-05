using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models.ViewModels;
using GwanjaLoveProto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static GwanjaLoveProto.Data.Implementations.GlobalHelpers;
using Microsoft.AspNetCore.Identity;

namespace GwanjaLoveProto.Controllers
{
    [Authorize(Roles = "Administrator, Developer")]
    public class OrderReceiveController : Controller
    {
        private readonly IUnitOfWork Uow;
        private readonly UserManager<AppUser> UserManager; 
        private AppUser? CurrentUser;

        public OrderReceiveController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        public async Task<IActionResult> Index(BaseFilters? filters)
        {
            List<OrderReceiveMethod> values = new List<OrderReceiveMethod>();
            if (filters != null)
            {
                values = await Uow.OrderReceiveMethodRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name.Contains(filters.Name)));
            }
            else
            {
                values = await Uow.OrderReceiveMethodRepository.GetAll();
            }

            return View(new GenericLandingPageViewModel<OrderReceiveMethod> { Items = values, SuccessfullPersistence = filters?.SuccessfulPersistence, Filters = filters ?? new BaseFilters() });
        }

        public async Task<IActionResult> OrderReceiveMethod(int? id)
        {
            try
            {
                if (id.HasValue)
                    return View(await Uow.OrderReceiveMethodRepository.FindAsync(id.Value));
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
                await Uow.OrderReceiveMethodRepository.DeleteAsync(id);
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
        public async Task<IActionResult> Add(OrderReceiveMethod orderReceiveMethod)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<OrderReceiveMethod>(ref orderReceiveMethod, true, CurrentUser.UserName);
                await Uow.OrderReceiveMethodRepository.AddAsync(orderReceiveMethod);
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
        public async Task<IActionResult> Update(OrderReceiveMethod orderReceiveMethod)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<OrderReceiveMethod>(ref orderReceiveMethod, orderReceiveMethod.Active, CurrentUser.UserName);
                Uow.OrderReceiveMethodRepository.Update(orderReceiveMethod);
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
