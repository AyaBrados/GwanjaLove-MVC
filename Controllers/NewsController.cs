using Microsoft.AspNetCore.Mvc;

namespace GwanjaLoveProto.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
