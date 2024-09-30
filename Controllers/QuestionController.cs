using Microsoft.AspNetCore.Mvc;

namespace GwanjaLoveProto.Controllers
{
    public class QuestionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
