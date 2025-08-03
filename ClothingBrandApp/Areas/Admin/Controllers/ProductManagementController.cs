using ClothingBrand.Services.Core.Admin.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.ProductManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using static ClothingBrandApp.GCommon.ApplicationConstants;
using static ClothingBrandApp.GCommon.ExceptionMessages;

namespace ClothingBrandApp.Web.Areas.Admin.Controllers
{
    public class ProductManagementController : BaseAdminController
    {
        private readonly IProductManagementService productManagementService;
        private readonly IWarehouseService warehouseService;
        private readonly ICategoryService categoryService;
        private readonly ILogger<ProductManagementController> logger;

        public ProductManagementController(IProductManagementService productManagementService, ICategoryService categoryService, IWarehouseService warehouseService, ILogger<ProductManagementController> logger)
        {
            this.productManagementService = productManagementService;
            this.categoryService = categoryService;
            this.warehouseService = warehouseService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            IEnumerable<ProductManagementIndexViewModel> allProducts = await this.productManagementService
                .GetProductManagementBoardDataAsync();

            return View(allProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ProductFormInputModel inputModel = new ProductFormInputModel()
            {
                Categories = await this.categoryService
                .GetAllCategoriesDropDownAsync(),
                Warehouses = await this.warehouseService
                .GetAllWarehousesDropDownAsync()
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.productManagementService.AddProductAsync(inputModel);
                TempData[SuccessMessageKey] = "Product added successfully!";

                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception e)
            {
                this.logger.LogCritical(e.Message);

                TempData[ErrorMessageKey] = "Fatal error occurred while adding your product! Please try again later!";
                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            try
            {
                ProductFormInputModel? editInputModel = await this.productManagementService
                    .GetProductForEditingAsync(id);
                if (editInputModel == null)
                {
                    return this.NotFound();
                }

                editInputModel.Categories = await this.categoryService.GetAllCategoriesDropDownAsync();
                editInputModel.Warehouses = await this.warehouseService.GetAllWarehousesDropDownAsync();

                return this.View(editInputModel);
            }
            catch (Exception e)
            {
                this.logger.LogCritical(e.Message);
                TempData[ErrorMessageKey] = "Fatal error occurred while updating the product! Please try again later!";

                return this.RedirectToAction(nameof(Manage));
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

                bool editSuccess = await this.productManagementService
                    .EditProductAsync(inputModel);
                if (!editSuccess)
                {
                    TempData[ErrorMessageKey] = "Selected Product does not exist!";
                }
                else
                {
                    TempData[SuccessMessageKey] = "Product updated successfully!";
                }

                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception e)
            {
                this.logger.LogCritical(e.Message);
                TempData[ErrorMessageKey] = "Fatal error occurred while updating the product! Please try again later!";

                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ToggleDelete(string? id)
        {
            Tuple<bool, bool> opResult = await this.productManagementService
                .DeleteOrRestoreProductAsync(id);
            bool success = opResult.Item1;
            bool isRestored = opResult.Item2;
            if (!success)
            {
                TempData[ErrorMessageKey] = "Product could not be found and updated!";
            }
            else
            {
                string operation = isRestored ? "restored" : "deleted";

                TempData[SuccessMessageKey] = $"Product {operation} successfully!";
            }

            return this.RedirectToAction(nameof(Manage));
        }
    }
}
