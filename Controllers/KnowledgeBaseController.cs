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
    public class KnowledgeBaseController : Controller
    {
        private readonly IUnitOfWork Uow;
        private readonly UserManager<AppUser> UserManager;
        private AppUser? CurrentUser;

        public KnowledgeBaseController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(BaseFilters? filters)
        {
            List<KnowledgeBase> values = new List<KnowledgeBase>();
            if (filters != null)
            {
                values = await Uow.KnowledgeBaseRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name.Contains(filters.Name))
                                                                                        && (filters.Category == null || a.CategoryId == filters.Category.Id));
            }
            else
            {
                values = await Uow.KnowledgeBaseRepository.GetAll();
            }

            return View(new GenericLandingPageViewModel<KnowledgeBase> { Items = values, SuccessfullPersistence = filters?.SuccessfulPersistence, Filters = filters ?? new BaseFilters() });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Term(int? id)
        {
            try
            {
                if (id.HasValue)
                    return View(await Uow.KnowledgeBaseRepository.FindAsync(id.Value));
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
                await Uow.KnowledgeBaseRepository.DeleteAsync(id);
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
        public async Task<IActionResult> Add(KnowledgeBase term)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<KnowledgeBase>(ref term, true, CurrentUser.UserName);
                await Uow.KnowledgeBaseRepository.AddAsync(term);
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
        public async Task<IActionResult> Update(KnowledgeBase term)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<KnowledgeBase>(ref term, term.Active, CurrentUser.UserName);
                Uow.KnowledgeBaseRepository.Update(term);
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
