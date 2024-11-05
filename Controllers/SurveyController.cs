using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models.ViewModels;
using GwanjaLoveProto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static GwanjaLoveProto.Data.Implementations.GlobalHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GwanjaLoveProto.Controllers
{
    [Authorize(Roles = "Administrator, Developer")]
    public class SurveyController : Controller
    {
        private readonly IUnitOfWork Uow;
        private readonly UserManager<AppUser> UserManager;
        private AppUser? CurrentUser;

        public SurveyController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(SurveyFilters? filters)
        {
            List<Survey> values = new List<Survey>();
            if (filters != null)
            {
                values = await Uow.SurveyRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name.Contains(filters.Name))
                                                                                        && (filters.Survey == null || a.Id == filters.Survey.Id));
            }
            else
            {
                values = await Uow.SurveyRepository.GetAll();
            }

            return View(new GenericLandingPageViewModel<Survey> { Items = values, SuccessfullPersistence = filters?.SuccessfulPersistence, Filters = filters ?? new SurveyFilters() });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Survey(int? id)
        {
            try
            {
                if (id.HasValue)
                    return View(await Uow.SurveyRepository.FindAsync(id.Value));
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
                await Uow.SurveyRepository.DeleteAsync(id);
                return RedirectToAction("Index", new SurveyFilters { SuccessfulPersistence = Uow.Save() });
            }
            catch
            {
                throw;
            }
        }

        public async Task<IActionResult> Add()
        {
            await GetQuestions();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(Survey survey)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<Survey>(ref survey, true, CurrentUser.UserName);
                await Uow.SurveyRepository.AddAsync(survey);
                return RedirectToAction("Index", new SurveyFilters { SuccessfulPersistence = Uow.Save() });
            }
            catch
            {
                throw;
            }
        }

        public async Task<IActionResult> Update()
        {
            await GetQuestions();
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Survey survey)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<Survey>(ref survey, survey.Active, CurrentUser.UserName);
                Uow.SurveyRepository.Update(survey);
                return RedirectToAction("Index", new SurveyFilters { SuccessfulPersistence = Uow.Save() });
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

        private async Task GetQuestions()
        {
            ViewBag.Questions = new SelectList(await Uow.QuestionRepository.GetAll()); 
        }
    }
}
