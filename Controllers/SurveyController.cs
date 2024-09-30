using Microsoft.AspNetCore.Mvc;

namespace GwanjaLoveProto.Controllers
{
    public class SurveyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
