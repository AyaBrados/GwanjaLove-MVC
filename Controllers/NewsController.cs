using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Models.ViewModels;
using GwanjaLoveProto.Models;
using Microsoft.AspNetCore.Mvc;
using GwanjaLoveProto.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using static GwanjaLoveProto.Data.Implementations.GlobalHelpers;
using Microsoft.AspNetCore.Identity;

namespace GwanjaLoveProto.Controllers
{
    [Authorize(Roles = "Administrator, Developer")]
    public class NewsController : Controller
    {
        private readonly IUnitOfWork Uow;
        private readonly UserManager<AppUser> UserManager;
        private AppUser? CurrentUser;

        public NewsController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(NewsFilters? filters)
        {
            List<News> values = new List<News>();
            if (filters != null)
            {
                values = await Uow.NewsRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name.Contains(filters.Name))
                                                                                        && (filters.UploadDateTime == null || a.UploadTime >= filters.UploadDateTime));
            }
            else
            {
                values = await Uow.NewsRepository.GetAll();
            }

            return View(new GenericLandingPageViewModel<News> { Items = values, SuccessfullPersistence = filters?.SuccessfulPersistence, Filters = filters ?? new NewsFilters() });
        }

        [AllowAnonymous]
        public async Task<IActionResult> News(int? id)
        {
            try
            {
                if (id.HasValue)
                    return View(await Uow.NewsRepository.FindAsync(id.Value));
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
                await Uow.NewsRepository.DeleteAsync(id);
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
        public async Task<IActionResult> Add(News news)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<News>(ref news, true, CurrentUser.UserName);
                await Uow.NewsRepository.AddAsync(news);
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
        public async Task<IActionResult> Update(News news)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<News>(ref news, news.Active, CurrentUser.UserName);
                Uow.NewsRepository.Update(news);
                return RedirectToAction("Index", Uow.Save());
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
