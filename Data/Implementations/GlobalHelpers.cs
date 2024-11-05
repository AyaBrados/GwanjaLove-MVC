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
            double gramsBought = 0;
            double repurchaseConstant = 0;
            foreach (var item in order.OrderProducts)
            {
                if (item.Product.Category.Name.Equals("Marijuana"))
                    gramsBought += item.ProductCount;
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
    }
}
