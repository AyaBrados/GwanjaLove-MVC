using GwanjaLoveProto.Data.ComponentFilters;
using GwanjaLoveProto.Data.Exceptions;
using GwanjaLoveProto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GwanjaLoveProto.Data.Implementations
{
    public static class GlobalHelpers
    {
        public static double CalculateCustomerLoyalty(Order order, List<Order> orderCount)
        {
            var constant = 0.8 * (orderCount.Count / 2);
            double gramsBought = 1;
            double repurchaseConstant = 1;
            foreach (var item in order.OrderProducts)
            {
                if (item.Product.Category.Name.Equals("Smokeables"))
                    gramsBought += item.ProductCount;
                else if (item.Product.Category.Name.Equals("Plant"))
                    gramsBought += 3;
                foreach (var product in orderCount)
                {
                    if (product.OrderProducts.Contains(item))
                        repurchaseConstant += 0.5;
                }
            }

            return constant * (gramsBought * 0.2) * repurchaseConstant;
        }

        public static void SetTransactionValues<T>(ref T model, bool? active, string? user) where T : BaseModel
        {
            model.Active = active.HasValue ? active.Value : true;
            model.SetupUser = string.IsNullOrEmpty(user) ? "Automation" : user;
            model.SetupDateTime = DateTime.Now;
        }

        public static string[] GetSortingEnums()
        {
            return Enum.GetNames(typeof(SortBy));
        }

        public static string? GetSortingValue(string? value)
        {
            return Enum.GetName(typeof(SortBy), value ?? "None");
        }

        public static string[] GetDirectionsEnum()
        {
            return Enum.GetNames(typeof(SortDirection));
        }

        public static void CheckCurrentUser(AppUser? currentUser, string? name)
        {
			if (currentUser == null)
				throw new EntityNotFoundException($"The current user could not be identified({name ?? string.Empty}), please sign in.");
		}
    }
}
