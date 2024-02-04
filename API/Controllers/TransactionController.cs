using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
