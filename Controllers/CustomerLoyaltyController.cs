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
    [Authorize]
    public class CustomerLoyaltyController : Controller
    {
        private readonly IUnitOfWork Uow;
        private readonly UserManager<AppUser> UserManager;
        private AppUser? CurrentUser;

        public CustomerLoyaltyController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(BaseFilters? filters)
        {
            List<CustomerLoyalty> values = new List<CustomerLoyalty>();
            var user = User;
            if (user.IsInRole("Customer"))
            {
                var innerUser = UserManager.Users.FirstOrDefault(x => !string.IsNullOrEmpty(x.UserName) && x.UserName.Equals(user.Identity.Name));
                var customer = innerUser != null ? await Uow.CustomerLoyaltyRepository.FirstOrDefaultAsync(a => a.UserId == innerUser.Id) : null;
                return RedirectToAction("CustomerLoyalty", customer?.Id);
            }
            else if (user.IsInRole("Administrator") || user.IsInRole("Developer"))
            {
                values = await Uow.CustomerLoyaltyRepository.GetAll();
            }

            return View(new GenericLandingPageViewModel<CustomerLoyalty> { Items = values, SuccessfullPersistence = filters?.SuccessfulPersistence });
        }

        [AllowAnonymous]
        public async Task<IActionResult> CustomerLoyalty(int? id)
        {
            try
            {
                if (id.HasValue)
                    return View(await Uow.CustomerLoyaltyRepository.FindAsync(id.Value));
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
                await Uow.CustomerLoyaltyRepository.DeleteAsync(id);
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
        public async Task<IActionResult> Add(CustomerLoyalty customerLoyalty)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<CustomerLoyalty>(ref customerLoyalty, true, CurrentUser.UserName);
                await Uow.CustomerLoyaltyRepository.AddAsync(customerLoyalty);
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
        public async Task<IActionResult> Update(CustomerLoyalty customerLoyalty)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<CustomerLoyalty>(ref customerLoyalty, customerLoyalty.Active, CurrentUser.UserName);
                Uow.CustomerLoyaltyRepository.Update(customerLoyalty);
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
