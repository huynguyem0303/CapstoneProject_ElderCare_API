using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PaymentController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
