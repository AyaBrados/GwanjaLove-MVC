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
    public class SurveyResponseController : Controller
    {
        private readonly IUnitOfWork Uow;
        private readonly UserManager<AppUser> UserManager;
        private AppUser? CurrentUser;

        public SurveyResponseController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(SurveyFilters? filters)
        {
            List<SurveyResponse> values = new List<SurveyResponse>();
            if (filters != null)
            {
                values = await Uow.SurveyResponseRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name.Contains(filters.Name))
                                                                                        && (filters.Survey == null || a.Id == filters.Survey.Id));
            }
            else
            {
                values = await Uow.SurveyResponseRepository.GetAll();
            }

            return View(new GenericLandingPageViewModel<SurveyResponse> { Items = values, SuccessfullPersistence = filters?.SuccessfulPersistence, Filters = filters ?? new SurveyFilters() });
        }

        [AllowAnonymous]
        public async Task<IActionResult> SurveyResponse(int? id)
        {
            try
            {
                if (id.HasValue)
                    return View(await Uow.SurveyResponseRepository.FindAsync(id.Value));
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
                await Uow.SurveyResponseRepository.DeleteAsync(id);
                return RedirectToAction("Index", new SurveyFilters { SuccessfulPersistence = Uow.Save() });
            }
            catch
            {
                throw;
            }
        }

        public async Task<IActionResult> Add(string surveyName)
        {
            await GetSurvey(surveyName);
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(SurveyResponse SurveyResponse)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<SurveyResponse>(ref SurveyResponse, true, CurrentUser?.UserName);
                await Uow.SurveyResponseRepository.AddAsync(SurveyResponse);
                return RedirectToAction("Index", new SurveyFilters { SuccessfulPersistence = Uow.Save() });
            }
            catch
            {
                throw;
            }
        }

        public async Task<IActionResult> Update(string surveyName)
        {
            await GetSurvey(surveyName);
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> Update(SurveyResponse SurveyResponse)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<SurveyResponse>(ref SurveyResponse, SurveyResponse.Active, CurrentUser?.UserName);
                Uow.SurveyResponseRepository.Update(SurveyResponse);
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

        private async Task GetSurvey(string surveyName)
        {
            ViewBag.Survey = await Uow.SurveyRepository.FirstOrDefaultAsync(x => x.Name.ToUpper().Equals(surveyName));
        }
    }
}
