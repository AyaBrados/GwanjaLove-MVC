using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models;
using GwanjaLoveProto.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GwanjaLoveProto.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductController : Controller
    {
        private readonly IRepositoryBase<Product> ProductRepository;

        public ProductController(IRepositoryBase<Product> productRepository)
        {
            ProductRepository = productRepository;
        }

        [AllowAnonymous]
        public IActionResult Index(object? filters)
        {
            List<Product> values = new List<Product>();
            bool filtersIsObject = filters?.GetType() != typeof(bool);
            if (filters != null && filtersIsObject)
                values = ProductRepository.GetFilteredCollection(filters);
            else
                values = ProductRepository.GetAll();

            return View(new GenericLandingPageViewModel<Product> { Items = values, SuccessfullPersistence = filters != null && !filtersIsObject ? (bool)filters : null });
        }

        [AllowAnonymous]
        public IActionResult Product(int id)
        {
            try
            {
                return View(ProductRepository.FindById(id));
            }
            catch
            {
                throw;
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                return View(ProductRepository.Delete(id));
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
        public IActionResult Add(Product product)
        {
            try
            {
                var success = ProductRepository.Add(product);
                return RedirectToAction("Index", success);
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
        public IActionResult Update(Product product)
        {
            try
            {
                var success = ProductRepository.Update(product);
                return RedirectToAction("Index", success);
            }
            catch
            {
                throw;
            }
        }
    }
}
