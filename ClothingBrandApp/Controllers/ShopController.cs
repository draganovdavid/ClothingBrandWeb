using ClothingBrand.Services.Core;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.Controllers;
using ClothingBrandApp.Web.ViewModels.Product;
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
        public async Task<IActionResult> Add()
        {
            try
            {
                ProductFormInputModel inputModel = new ProductFormInputModel()
                {
                    Categories = await this.categoryService.GetAllCategoriesDropDownAsync()
                };
                return this.View(inputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductFormInputModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(inputModel);
                }

                bool addResult = await this.shopService
                    .AddProductAsync(this.GetUserId()!, inputModel);
                if (addResult == false)
                {
                    this.ModelState.AddModelError(string.Empty, ServiceCreateErrorMessage);
                    return this.View(inputModel);
                }
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
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
        public async Task<IActionResult> Edit(Guid? id)
        {
            try
            {
                ProductFormInputModel? editInputModel = await this.shopService
                    .GetProductForEditingAsync(id);
                if (editInputModel == null)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                editInputModel.Categories = await this.categoryService
                    .GetAllCategoriesDropDownAsync();

                return this.View(editInputModel);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductFormInputModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(inputModel);
                }

                bool editResult = await this.shopService
                    .EditProductAsync(this.GetUserId()!, inputModel);
                if (!editResult)
                {
                    this.ModelState.AddModelError(string.Empty, ServiceCreateErrorMessage);
                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });
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
        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {
                ProductDeleteInputModel? deleteInputModel = await this.shopService
                    .GetProductForDeleteAsync(id);
                if (deleteInputModel == null)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(deleteInputModel);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductDeleteInputModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    //ModelState.AddModelError(string.Empty, "Please do not modify the page!");
                    ModelState.AddModelError(string.Empty, "Product not found or already deleted.");
                    return View(inputModel);
                }

                bool deleteResult = await this.shopService
                    .SoftDeleteAsync(inputModel);
                if (!deleteResult)
                {
                    this.ModelState.AddModelError(string.Empty, "A fatal error occurred while deleting your product! Please try again later!");
                    // Fix
                    return View(inputModel);
                }

                return this.RedirectToAction(nameof(Index));
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
