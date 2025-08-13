using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models.ViewModels;
using GwanjaLoveProto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using static GwanjaLoveProto.Data.Implementations.GlobalHelpers;
using GwanjaLoveProto.Data.Exceptions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GwanjaLoveProto.Controllers
{
    [Authorize(Roles = "Administrator, Developer")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork Uow;
        private readonly UserManager<AppUser> UserManager;
        private AppUser? CurrentUser;

        public OrderController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        public async Task<IActionResult> Index(OrderFilters? filters)
        {
            await GetCurrentUser();
            PopulateUsers(filters?.User);
            List<Order> values = new List<Order>();
            if (filters != null)
            {
                if (CurrentUser != null && await UserManager.IsInRoleAsync(CurrentUser, "Customer"))
                    filters.User = CurrentUser;

                values = await Uow.OrderRepository.GetFilteredCollectionAsync(a => (string.IsNullOrEmpty(filters.Name) || a.Name.Contains(filters.Name))
                                                                                        && (!filters.PkId.HasValue || a.Id == filters.PkId)
                                                                                        && (!filters.OrderDate.HasValue || a.OrderDate >= filters.OrderDate)
                                                                                        && (filters.User == null || a.UserId.Equals(filters.User.Id))
                                                                                        && (!filters.Received.HasValue || a.OrderReceived == filters.Received));
            }
            else
            {
                values = await Uow.OrderRepository.GetAll();
            }

            return View(new GenericLandingPageViewModel<Order> { Items = values, SuccessfullPersistence = filters?.SuccessfullPersistence, Filters = filters ?? new OrderFilters() });
        }

        public async Task<IActionResult> Order(int? id)
        {
            try
            {
                if (id.HasValue)
                    return View(await Uow.OrderRepository.FindAsync(id.Value));
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
                var order = await Uow.OrderRepository.FindAsync(id);
                await Uow.OrderRepository.DeleteAsync(id);
				return RedirectToAction("Index", new OrderFilters
				{
					SuccessfullPersistence = new SuccessfullPersistenceViewModel
					{
						SuccessfulPersistence = Uow.Save(),
						EntityName = $"Order: {order?.Name} successfully deleted."
					}
				});
			}
            catch
            {
                throw;
            }
        }

        public IActionResult Checkout()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Checkout(Cart cart)
        {
            try
            {
                await GetCurrentUser();
                CheckCurrentUser(CurrentUser, User.Identity?.Name);

                var orderCount = await Uow.OrderRepository.GetFilteredCollectionAsync(x => CurrentUser == null || x.UserId == CurrentUser.Id);
                // Name is constructed through: Date of Order + orderCount for user + userName
                var order = new Order
                {
                    Name = $"{DateTime.Now} {orderCount.Count + 1} {CurrentUser?.UserName}",
                    Description = $"{DateTime.Now} {orderCount.Count + 1} {CurrentUser?.UserName}",
                    UserId = CurrentUser.Id,
				};

                orderCount.Add(order);
                SetTransactionValues<Order>(ref order, true, CurrentUser.UserName);

                await Uow.OrderRepository.AddAsync(order);
                cart.OrderId = order.Id;
                Uow.CartRepository.Update(cart);

                Dictionary<int, OrderProduct> orderedProducts = await ProcessOrderedGoods(cart, order, orderCount);
                
                await Uow.OrderProductRepository.AddRangeAsync(orderedProducts.Values);

				return RedirectToAction("Index", new OrderFilters
				{
					SuccessfullPersistence = new SuccessfullPersistenceViewModel
					{
						SuccessfulPersistence = Uow.Save(),
						EntityName = $"Order: {order?.Name} successfully created."
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
        public async Task<IActionResult> Update(Order order)
        {
            try
            {
                await GetCurrentUser();
                SetTransactionValues<Order>(ref order, order.Active, CurrentUser.UserName);
                Uow.OrderRepository.Update(order);
                var orderCount = await Uow.OrderRepository.GetFilteredCollectionAsync(x => CurrentUser == null || x.UserId == CurrentUser.Id);
                await HandleCustomerLoyalty(order, orderCount);
				return RedirectToAction("Index", new OrderFilters
				{
					SuccessfullPersistence = new SuccessfullPersistenceViewModel
					{
						SuccessfulPersistence = Uow.Save(),
						EntityName = $"Order: {order?.Name} successfully updated."
					}
				});
			}
            catch
            {
                throw;
            }
        }

        private void PopulateUsers(AppUser? user)
        {
            ViewBag.Users = new SelectList(UserManager.Users.ToList(), user);
        }

        private async Task HandleCustomerLoyalty(Order order, List<Order> orderCount)
        {
            var customerLoyalty = await Uow.CustomerLoyaltyRepository.FirstOrDefaultAsync(x => x.UserId == CurrentUser.Id);

            CustomerLoyalty loyalty = customerLoyalty ?? new CustomerLoyalty
            {
                Name = CurrentUser.UserName,
                Description = $"Loyalty for {CurrentUser.UserName}",
                UserId = CurrentUser.Id
            };

            SetTransactionValues<CustomerLoyalty>(ref loyalty, customerLoyalty?.Active, CurrentUser?.UserName);
            loyalty.LoyaltyPoints = CalculateCustomerLoyalty(order, orderCount);

            if (customerLoyalty != null)
            {
                Uow.CustomerLoyaltyRepository.Update(loyalty);
            }
            else
            {
                await Uow.CustomerLoyaltyRepository.AddAsync(loyalty);
            }
        }

        private async Task<Dictionary<int, OrderProduct>> ProcessOrderedGoods(Cart cart, Order order, List<Order> orderCount)
        {
			Dictionary<int, OrderProduct> orderedProducts = new Dictionary<int, OrderProduct>();
            var existingOrderProducts = await Uow.OrderProductRepository.GetFilteredCollectionAsync(x => x.OrderId == order.Id);

			foreach (var orderedProduct in cart.Products)
			{
				var actualP = await Uow.ProductRepository.FindAsync(orderedProduct.Id);
                var existingOrderP = existingOrderProducts.FirstOrDefault(x =>  x.OrderId == order.Id && x.ProductId == actualP.Id);

				OrderProduct product = existingOrderP != null ? existingOrderP : new OrderProduct()
				{
					Name = order.Name,
					Description = $"{order.Name} - {actualP?.Name}",
					OrderId = order.Id,
					ProductId = orderedProduct.Id,
				};

				if (orderedProducts.ContainsKey(orderedProduct.Id))
                {
					orderedProducts[orderedProduct.Id].ProductCount += 1;
                }
				else
                {
                    product.ProductCount = 1;
					orderedProducts.Add(orderedProduct.Id, product);
                }

                if (existingOrderP != null)
                {
                    product.ProductCount = cart.Products.Where(x => x.Id == actualP.Id).Count() + existingOrderProducts.Where(x => x.ProductId == actualP.Id).Count();
                    Uow.OrderProductRepository.Update(product);
                }
                else
                {
                    await Uow.OrderProductRepository.AddAsync(product);
                }
			}

			await HandleCustomerLoyalty(order, orderCount);

            return orderedProducts;
		}

		private async Task GetCurrentUser()
        {
            string? userName = User.Identity?.Name;
            CurrentUser = await UserManager.FindByNameAsync(userName ?? "");
        }
    }
}
