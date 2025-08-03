using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.Controllers;
using ClothingBrandApp.Web.ViewModels.Admin.ProductManagement;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ClothingBrandApp.Web.ViewModels.ValidationMessages.Product;

namespace ClothingBrand.Web.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IShopService shopService;
        private readonly ICategoryService categoryService;
        private readonly IFavoriteService favoriteService;
        private readonly IShoppingCartService shoppingCartService;

        public ShopController(IShopService shopService,
            ICategoryService categoryService, IFavoriteService favoriteService, IShoppingCartService shoppingCartService)
        {
            this.shopService = shopService;
            this.categoryService = categoryService;
            this.favoriteService = favoriteService;
            this.shoppingCartService = shoppingCartService;        
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductIndexViewModel> allProducts = await shopService
                .GetAllProductsAsync();
            if (this.IsUserAuthenticated())
            {
                foreach (ProductIndexViewModel productIndexVM in allProducts)
                {
                    productIndexVM.IsFavorite = await this.favoriteService
                        .IsProductAddedToFavorites(productIndexVM.Id, this.GetUserId()!);
                    productIndexVM.IsInShoppingCart = await this.shoppingCartService
                        .IsProductAddedToShoppingCart(productIndexVM.Id, this.GetUserId()!);
                }
            }
            return View(allProducts);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            try
            {
                ProductDetailsViewModel? movieDetails = await this.shopService
                    .GetProductDetailsByIdAsync(id);
                if (this.IsUserAuthenticated() && movieDetails != null)
                {
                    movieDetails.IsFavorite = await this.favoriteService
                            .IsProductAddedToFavorites(movieDetails.Id, this.GetUserId()!);
                    movieDetails.IsInShoppingCart = await this.shoppingCartService
                        .IsProductAddedToShoppingCart(movieDetails.Id, this.GetUserId()!);

                }
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> MenCollection()
        {
            const string  genderName = "Men";
            IEnumerable<ProductIndexViewModel> allProducts = await shopService
                .GetProductsByGenderAsync(genderName);
            if (this.IsUserAuthenticated())
            {
                foreach (ProductIndexViewModel productIndexVM in allProducts)
                {
                    productIndexVM.IsFavorite = await this.favoriteService
                        .IsProductAddedToFavorites(productIndexVM.Id, this.GetUserId()!);
                    productIndexVM.IsInShoppingCart = await this.shoppingCartService
                        .IsProductAddedToShoppingCart(productIndexVM.Id, this.GetUserId()!);
                }
            }
            return View(allProducts);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> WomenCollection()
        {
            const string genderName = "Women";
            IEnumerable<ProductIndexViewModel> allProducts = await shopService
                .GetProductsByGenderAsync(genderName);
            if (this.IsUserAuthenticated())
            {
                foreach (ProductIndexViewModel productIndexVM in allProducts)
                {
                    productIndexVM.IsFavorite = await this.favoriteService
                        .IsProductAddedToFavorites(productIndexVM.Id, this.GetUserId()!);
                    productIndexVM.IsInShoppingCart = await this.shoppingCartService
                        .IsProductAddedToShoppingCart(productIndexVM.Id, this.GetUserId()!);
                }
            }
            return View(allProducts);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> KidsCollection()
        {
            const string genderName = "Kids";
            IEnumerable<ProductIndexViewModel> allProducts = await shopService
                .GetProductsByGenderAsync(genderName);
            if (this.IsUserAuthenticated())
            {
                foreach (ProductIndexViewModel productIndexVM in allProducts)
                {
                    productIndexVM.IsFavorite = await this.favoriteService
                        .IsProductAddedToFavorites(productIndexVM.Id, this.GetUserId()!);
                    productIndexVM.IsInShoppingCart = await this.shoppingCartService
                        .IsProductAddedToShoppingCart(productIndexVM.Id, this.GetUserId()!);
                }
            }
            return View(allProducts);
        }
    }
}
