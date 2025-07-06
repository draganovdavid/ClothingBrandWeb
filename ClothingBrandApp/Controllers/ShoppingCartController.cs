using ClothingBrand.Services.Core;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrandApp.Web.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService shoppingCart;

        public ShoppingCartController(IShoppingCartService shoppingCart)
        {
            this.shoppingCart = shoppingCart;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                string? userId = this.GetUserId();
                if (userId == null)
                {
                    return this.Forbid();
                }

                IEnumerable<ProductIndexViewModel>? userShoopingCartProducts = await this.shoppingCart
                    .GetAllProductsInShoppingCartAsync(userId);
                
                return this.View(userShoopingCartProducts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid? productId)
        {
            try
            {
                string? userId = this.GetUserId();
                if (userId == null)
                {
                    return this.Forbid();
                }

                bool wasAdded = await this.shoppingCart
                    .AddProductToShoppingCartAsync(productId, userId);

                if (!wasAdded)
                {
                    return this.RedirectToAction(nameof(Index), "Shop");
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid? productId)
        {
            try
            {
                string? userId = this.GetUserId();
                if (userId == null)
                {
                    return this.Forbid();
                }

                bool result = await this.shoppingCart
                    .DeleteProductFromShoppingCartAsync(productId, userId);
                if (result == false)
                {
                    // TODO: Add JS notifications
                    return this.RedirectToAction(nameof(Index), "Shop");
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        
    }
}
