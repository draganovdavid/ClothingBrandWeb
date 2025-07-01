using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Product;
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

        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                ProductDetailsViewModel? movieDetails = await this.shopService
                    .GetProductDetailsByIdAsync(id);
                if (movieDetails == null)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(movieDetails);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
