using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models;
using GwanjaLoveProto.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static GwanjaLoveProto.Data.Implementations.GlobalHelpers;

namespace GwanjaLoveProto.Controllers
{
	public class UserFavouriteController : Controller
	{
		private readonly IUnitOfWork Uow;
		private readonly UserManager<AppUser> _userManager;
		private AppUser? CurrentUser;

		public UserFavouriteController(IUnitOfWork uow, UserManager<AppUser> userManager)
		{
			Uow = uow;
			_userManager = userManager;
		}

		public async Task<IActionResult> Favourite(Product product)
		{
			await GetCurrentUser();
			CheckCurrentUser(CurrentUser, User.Identity?.Name);

			var favourite = await Uow.UserFavouriteRepository.FirstOrDefaultAsync(x => x.UserId == CurrentUser.Id && x.ProductId == product.Id);

			if (favourite == null)
			{
				var userFavourite = new UserFavourite
				{
					Name = product.Name,
					Description = $"{CurrentUser.UserName} - {product.Name}",
					UserId = CurrentUser.Id,
					ProductId = product.Id
				};

				SetTransactionValues<UserFavourite>(ref userFavourite, true, CurrentUser.UserName);
				await Uow.UserFavouriteRepository.AddAsync(userFavourite);
			}
			else
			{
				await Uow.UserFavouriteRepository.DeleteAsync(favourite.Id);
			}

			var saved = Uow.Save();
			return View(new GenericLandingPageViewModel<UserFavourite>
			{
				Items = new List<UserFavourite>(),
				Filters = new BaseFilters(),
				SuccessfullPersistence = new SuccessfullPersistenceViewModel
				{
					EntityName = $"{product.Name} has been {(saved ? "successfully" : "unsuccessfully")} {(favourite == null ? "favourited" : "unfavourited")}",
					SuccessfulPersistence = saved
				}
			});
		}

		private async Task GetCurrentUser()
		{
			string? userName = User.Identity?.Name;
			CurrentUser = await _userManager.FindByNameAsync(userName ?? "");
		}
	}
}
