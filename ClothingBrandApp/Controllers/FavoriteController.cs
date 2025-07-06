using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrandApp.Web.Controllers
{
    public class FavoriteController : BaseController
    {
        private readonly IFavoriteService favoriteService;
        public FavoriteController(IFavoriteService favoriteService) 
        {
            this.favoriteService = favoriteService;
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

                IEnumerable<ProductIndexViewModel>? userFavProducts = await this.favoriteService
                    .GetUserFavoriteProductsAsync(userId);
                if (this.IsUserAuthenticated() && userFavProducts != null)
                {
                    foreach (ProductIndexViewModel productIndexVM in userFavProducts)
                    {
                        productIndexVM.IsFavorite = await this.favoriteService
                            .IsProductAddedToFavorites(productIndexVM.Id, this.GetUserId());
                    }
                }
                return this.View(userFavProducts);
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

                await this.favoriteService
                    .AddProductToUserFavoritesAsync(productId, userId);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index), "Shop");
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
                    // Not a valid case, added as defensive mechanism
                    return this.Forbid();
                }

                bool result = await this.favoriteService
                    .DeleteProductFromUserFavoritesAsync(productId, userId);
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

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
