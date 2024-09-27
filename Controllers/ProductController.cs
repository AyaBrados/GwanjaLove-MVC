using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models;
using Microsoft.AspNetCore.Mvc;

namespace GwanjaLoveProto.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepositoryBase<Product> ProductRepository;

        public ProductController(IRepositoryBase<Product> productRepository)
        {
            ProductRepository = productRepository;
        }

        public IActionResult Index(object? filters)
        {
            List<Product> values = new List<Product>();
            if (filters != null)
                values = ProductRepository.GetFilteredCollection(filters);
            else
                values = ProductRepository.GetAll();
            return View();
        }
    }
}
