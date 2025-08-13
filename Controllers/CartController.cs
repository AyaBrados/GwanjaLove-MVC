using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models;
using GwanjaLoveProto.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static GwanjaLoveProto.Data.Implementations.GlobalHelpers;

namespace GwanjaLoveProto.Controllers
{
	public class CartController : Controller
	{
		private readonly IUnitOfWork Uow;
		private readonly UserManager<AppUser> UserManager;
		private AppUser? CurrentUser;

		public CartController(IUnitOfWork uow, UserManager<AppUser> userManager)
		{
			Uow = uow;
			UserManager = userManager;
		}

		// allCarts = false indicates the user wants to see only the current cart
		[HttpGet]
		public async Task<IActionResult> Cart(bool allCarts = false)
		{
			await GetCurrentUser();
			CheckCurrentUser(CurrentUser, User.Identity?.Name);
			List<Cart> carts = await Uow.CartRepository.GetFilteredCollectionAsync(x => x.UserId == CurrentUser.Id);

			// OrderId = NULL means the cart has not been checked out yet
			if (!allCarts)
			{
				carts.RemoveAll(x => x.OrderId.HasValue);
			}
			return View(new GenericLandingPageViewModel<Cart> { Items = carts, SuccessfullPersistence = new SuccessfullPersistenceViewModel(), Filters = new BaseFilters() });
		}

		[HttpPost]
		public async Task<IActionResult> AddToCart(Product? product, ShopSpecial? special)
		{
			await GetCurrentUser();
			CheckCurrentUser(CurrentUser, User.Identity?.Name);
			var carts = await Uow.CartRepository.GetFilteredCollectionAsync(x => x.UserId == CurrentUser.Id);

			var cart = carts.Where(x => !x.OrderId.HasValue).First();
			var currentCart = cart ?? new Cart
			{
				Name = $"{CurrentUser.UserName}'s Cart",
				Description = $"{CurrentUser.UserName}'s Cart created on {DateTime.Now}",
				UserId = CurrentUser.Id,
			};

			if (product != null)
				currentCart.Products.Add(product);
			else if (special != null)
				currentCart.Specials.Add(special);

			if (cart != null)
				Uow.CartRepository.Update(currentCart);
			else
				await Uow.CartRepository.AddAsync(currentCart);

			string message = cart != null && (product != null || special != null) ? $"{(product?.Name ?? special?.Name)} was successfully added to your cart"
								: cart == null && (product != null || special != null) ? $"A new cart was created and {(product?.Name ?? special?.Name)} was successfully added to your cart"
								: "Sorry something went wrong, can you please try again?";

			return View(new GenericLandingPageViewModel<Cart> 
			{ 
				Items = new List<Cart>(), 
				SuccessfullPersistence = new SuccessfullPersistenceViewModel
				{
					EntityName = message,
					SuccessfulPersistence = Uow.Save()
				}, 
				Filters = new BaseFilters() 
			});
		}

		public async Task<IActionResult> RemoveFromCart(Product? product, ShopSpecial special)
		{
			try
			{
				if (product == null && special == null)
					throw new InvalidOperationException("Product/Special cannot be null, please retry the remove operation.");

				await GetCurrentUser();
				CheckCurrentUser(CurrentUser, User.Identity?.Name);

				var item = await Uow.CartRepository.FirstOrDefaultAsync(x => x.UserId == CurrentUser.Id && x.OrderId == null);
				if (product != null && item != null)
					item.Products.Remove(product);
				else if (special != null && item != null)
					item.Specials.Remove(special);

				if (item != null && (item.Products.Any() || item.Specials.Any()))
					Uow.CartRepository.Update(item);
				else
					await Uow.CartRepository.DeleteAsync(item.Id);

				Uow.Save();
				return RedirectToAction("Cart");
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
