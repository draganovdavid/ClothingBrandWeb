using Microsoft.AspNetCore.Mvc;

namespace ClothingBrand.Web.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
