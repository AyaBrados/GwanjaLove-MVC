using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GwanjaLoveProto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _uow;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _uow = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            await GetCategories();
            await GetNews();
            await GetSpecials();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task GetCategories()
        {
            var trendingProducts = (await _uow.UserFavouriteRepository.GetAll()).GroupBy(x => x.ProductId).OrderByDescending(x => x.Count())
                                    .Select(x => x.Key).Take(10).ToList();
            ViewBag.Products = (await _uow.ProductRepository.GetFilteredCollectionAsync(x => trendingProducts.Contains(x.Id) && x.Active == true)).ToList();
        }

        private async Task GetNews()
        {
            ViewBag.News = (await _uow.NewsRepository.GetFilteredCollectionAsync(x => x.UploadTime >= DateTime.Now.AddDays(-30))).ToList();
        }

        private async Task GetSpecials()
        {
            ViewBag.Specials = (await _uow.ShopSpecialRepository.GetFilteredCollectionAsync(x => x.SpecialEndDate <= DateTime.Now.AddDays(-30))).ToList();
        }
    }
}
