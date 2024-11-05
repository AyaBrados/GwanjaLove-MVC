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
    public class PaymentMethodController : Controller
    {
        private readonly IUnitOfWork Uow;
        private readonly UserManager<AppUser> UserManager;
        private AppUser? CurrentUser;

        public PaymentMethodController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        public async Task<IActionResult> Index(BaseFilters? filters)
        {
            List<PaymentMethod> values = new List<PaymentMethod>();
            if (filters != null)
            {
                values = await Uow.PaymentMethodRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name.Contains(filters.Name)));
            }
            else
            {
                values = await Uow.PaymentMethodRepository.GetAll();
            }

            return View(new GenericLandingPageViewModel<PaymentMethod> { Items = values, SuccessfullPersistence = filters?.SuccessfulPersistence, Filters = filters ?? new BaseFilters() });
        }

        public async Task<IActionResult> PaymentMethod(int? id)
        {
            try
            {
                if (id.HasValue)
                    return View(await Uow.PaymentMethodRepository.FindAsync(id.Value));
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
                await Uow.PaymentMethodRepository.DeleteAsync(id);
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
        public async Task<IActionResult> Add(PaymentMethod paymentMethod)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<PaymentMethod>(ref paymentMethod, true, CurrentUser.UserName);
                await Uow.PaymentMethodRepository.AddAsync(paymentMethod);
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
        public async Task<IActionResult> Update(PaymentMethod paymentMethod)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<PaymentMethod>(ref paymentMethod, paymentMethod.Active, CurrentUser.UserName);
                Uow.PaymentMethodRepository.Update(paymentMethod);
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
