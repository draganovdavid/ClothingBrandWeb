using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.Controllers;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using static ClothingBrandApp.Web.ViewModels.ValidationMessages.Product;

namespace ClothingBrand.Web.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IShopService shopService;
        private readonly ICategoryService categoryService;

        public ShopController(IShopService shopService, ICategoryService categoryService)
        {
            this.shopService = shopService;
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await shopService
                .GetAllProductsAsync();

            return View(products);
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
                    return this.RedirectToAction(nameof(Create));
                }

                bool createResult = await this.shopService
                    .AddProductAsync(this.GetUserId()!, inputModel);
                if (createResult == false)
                {
                    return this.RedirectToAction(nameof(Create));
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
