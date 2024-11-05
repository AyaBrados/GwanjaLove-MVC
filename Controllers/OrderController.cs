using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models.ViewModels;
using GwanjaLoveProto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using static GwanjaLoveProto.Data.Implementations.GlobalHelpers;

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

            return View(new GenericLandingPageViewModel<Order> { Items = values, SuccessfullPersistence = filters?.SuccessfulPersistence, Filters = filters ?? new OrderFilters() });
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
                await Uow.OrderRepository.DeleteAsync(id);
                return RedirectToAction("Index", new OrderFilters { SuccessfulPersistence = Uow.Save() });
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
        public async Task<IActionResult> Checkout(Order order)
        {
            try
            {
                await GetCurrentUser();
                var orderCount = await Uow.OrderRepository.GetFilteredCollectionAsync(x => CurrentUser == null || x.UserId == CurrentUser.Id);
                orderCount.Add(order);
                // Name is constructed through: Date of Order + orderCount for user + userName
                order.Name = $"{DateTime.Now} {orderCount.Count} {CurrentUser?.UserName}";
                order.Description = order.Name;
                SetTransactionValues<Order>(ref order, true, CurrentUser.UserName);
                await Uow.OrderRepository.AddAsync(order);
                Dictionary<int, OrderProduct> orderedProducts = new Dictionary<int, OrderProduct>();
                foreach (var orderedProduct in order.OrderProducts)
                {
                    var actualP = await Uow.ProductRepository.FindAsync(orderedProduct.Id);
                    var product = new OrderProduct() 
                    { 
                        Name = order.Name,
                        Description = $"{order.Name} - {actualP?.Name}",
                        OrderId = order.Id,
                        ProductId = orderedProduct.Id,
                    };
                    if (orderedProducts.ContainsKey(orderedProduct.Id))
                        orderedProducts[orderedProduct.Id].ProductCount += 1;
                    else
                        orderedProducts.Add(orderedProduct.Id, product);
                }
                await Uow.OrderProductRepository.AddRangeAsync(orderedProducts.Values);
                await HandleCustomerLoyalty(order, orderCount);

                return RedirectToAction("Index", new OrderFilters { SuccessfulPersistence = Uow.Save() });
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
                return RedirectToAction("Index", new OrderFilters { SuccessfulPersistence = Uow.Save() });
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
            var customerLoyalties = await Uow.CustomerLoyaltyRepository.GetFilteredCollectionAsync(x => (CurrentUser == null || x.UserId == CurrentUser.Id));
            var customerLoyalty = customerLoyalties.FirstOrDefault();
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

        private async Task GetCurrentUser()
        {
            string? userName = User.Identity?.Name;
            CurrentUser = await UserManager.FindByNameAsync(userName ?? "");
        }
    }
}
