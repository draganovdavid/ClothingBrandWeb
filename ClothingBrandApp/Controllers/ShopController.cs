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
    }
}
