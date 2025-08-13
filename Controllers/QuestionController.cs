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
    public class QuestionController : Controller
    {
        private readonly IUnitOfWork Uow;
        private readonly UserManager<AppUser> UserManager;
        private AppUser? CurrentUser;

        public QuestionController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        public async Task<IActionResult> Index(SurveyFilters? filters)
        {
            List<Question> values = new List<Question>();
            if (filters != null)
            {
                values = await Uow.QuestionRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name.Contains(filters.Name))
                                                                                        && (filters.Survey == null || a.SurveyId == filters.Survey.Id));
            }
            else
            {
                values = await Uow.QuestionRepository.GetAll();
            }

            return View(new GenericLandingPageViewModel<Question> { Items = values, SuccessfullPersistence = filters?.SuccessfullPersistence, Filters = filters ?? new SurveyFilters() });
        }

        public async Task<IActionResult> Question(int? id)
        {
            try
            {
                if (id.HasValue)
                    return View(await Uow.QuestionRepository.FindAsync(id.Value));
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
                var question = await Uow.QuestionRepository.FindAsync(id);
                await Uow.QuestionRepository.DeleteAsync(id);
                return RedirectToAction("Index", new SurveyFilters
				{
					SuccessfullPersistence = new SuccessfullPersistenceViewModel
					{
						SuccessfulPersistence = Uow.Save(),
						EntityName = $"Payment Method: {question?.Name} successfully deleted."
					}
				});
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
        public async Task<IActionResult> Add(Question question)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<Question>(ref question, true, CurrentUser.UserName);
                await Uow.QuestionRepository.AddAsync(question);
                return RedirectToAction("Index", new SurveyFilters
				{
					SuccessfullPersistence = new SuccessfullPersistenceViewModel
					{
						SuccessfulPersistence = Uow.Save(),
						EntityName = $"Payment Method: {question?.Name} successfully added."
					}
				});
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
        public async Task<IActionResult> Update(Question question)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<Question>(ref question, question.Active, CurrentUser.UserName);
                Uow.QuestionRepository.Update(question);
                return RedirectToAction("Index", new SurveyFilters
				{
					SuccessfullPersistence = new SuccessfullPersistenceViewModel
					{
						SuccessfulPersistence = Uow.Save(),
						EntityName = $"Payment Method: {question?.Name} successfully updated."
					}
				});
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
