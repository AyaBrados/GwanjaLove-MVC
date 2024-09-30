using Microsoft.AspNetCore.Mvc;

namespace GwanjaLoveProto.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
