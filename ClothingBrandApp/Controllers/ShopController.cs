using ClothingBrand.Services.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrand.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IShopService shopService;

        public ShopController(IShopService shopService)
        {
            this.shopService = shopService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await shopService
                .GetAllProductsAsync();

            return View(products);
        }
    }
}
