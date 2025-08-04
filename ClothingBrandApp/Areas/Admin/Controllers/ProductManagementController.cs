using ClothingBrand.Services.Core.Admin.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.ProductManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ClothingBrandApp.GCommon.ApplicationConstants;

namespace ClothingBrandApp.Web.Areas.Admin.Controllers
{
    [Area(AdminRoleName)]
    [Authorize]
    public class ProductManagementController : Controller
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

        [Authorize(Roles = AdminRoleName)]
        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var allProducts = await this.productManagementService.GetProductManagementBoardDataAsync();
            return View(allProducts);
        }

        [Authorize(Roles = $"{AdminRoleName},{ManagerRoleName}")]
        [HttpGet]
        public async Task<IActionResult> Create(string? warehouseId)
        {
            var inputModel = new ProductFormInputModel
            {
                Categories = await this.categoryService.GetAllCategoriesDropDownAsync(),
                Warehouses = await this.warehouseService.GetAllWarehousesDropDownAsync()
            };

            ViewBag.WarehouseId = warehouseId;

            return this.View(inputModel);
        }

        [Authorize(Roles = $"{AdminRoleName},{ManagerRoleName}")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductFormInputModel inputModel, string? warehouseId)
        {
            if (!this.ModelState.IsValid)
            {
                ViewBag.WarehouseId = warehouseId;
                return this.View(inputModel);
            }

            try
            {
                await this.productManagementService.AddProductAsync(inputModel);
                TempData[SuccessMessageKey] = "Product added successfully!";

                if (User.IsInRole(ManagerRoleName) && !string.IsNullOrEmpty(warehouseId))
                {
                    return RedirectToAction("Stock", "Warehouse", new { area = "", id = warehouseId });
                }

                return RedirectToAction(nameof(Manage));
            }
            catch (Exception e)
            {
                this.logger.LogCritical(e.Message);
                TempData[ErrorMessageKey] = "Fatal error occurred while adding your product! Please try again later!";
                return RedirectToAction(nameof(Manage));
            }
        }

        [Authorize(Roles = $"{AdminRoleName},{ManagerRoleName}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id, string? warehouseId)
        {
            try
            {
                var editInputModel = await this.productManagementService.GetProductForEditingAsync(id);
                if (editInputModel == null)
                {
                    return this.NotFound();
                }

                editInputModel.Categories = await this.categoryService.GetAllCategoriesDropDownAsync();
                editInputModel.Warehouses = await this.warehouseService.GetAllWarehousesDropDownAsync();

                ViewBag.WarehouseId = warehouseId;

                return this.View(editInputModel);
            }
            catch (Exception e)
            {
                this.logger.LogCritical(e.Message);
                TempData[ErrorMessageKey] = "Fatal error occurred while updating the product! Please try again later!";
                return this.RedirectToAction(nameof(Manage));
            }
        }

        [Authorize(Roles = $"{AdminRoleName},{ManagerRoleName}")]
        [HttpPost]
        public async Task<IActionResult> Edit(ProductFormInputModel inputModel, string? warehouseId)
        {
            if (!this.ModelState.IsValid)
            {
                ViewBag.WarehouseId = warehouseId;
                return this.View(inputModel);
            }

            try
            {
                bool editSuccess = await this.productManagementService.EditProductAsync(inputModel);
                if (!editSuccess)
                {
                    TempData[ErrorMessageKey] = "Selected Product does not exist!";
                }
                else
                {
                    TempData[SuccessMessageKey] = "Product updated successfully!";
                }

                if (User.IsInRole(ManagerRoleName) && !string.IsNullOrEmpty(warehouseId))
                {
                    return RedirectToAction("Stock", "Warehouse", new { area = "", id = warehouseId });
                }

                return RedirectToAction(nameof(Manage));
            }
            catch (Exception e)
            {
                this.logger.LogCritical(e.Message);
                TempData[ErrorMessageKey] = "Fatal error occurred while updating the product! Please try again later!";
                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{AdminRoleName},{ManagerRoleName}")]
        public async Task<IActionResult> ToggleDelete(string? id, string? warehouseId)
        {
            var opResult = await this.productManagementService.DeleteOrRestoreProductAsync(id);
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

            if (User.IsInRole(ManagerRoleName) && !string.IsNullOrEmpty(warehouseId))
            {
                return RedirectToAction("Stock", "Warehouse", new { area = "", id = warehouseId });
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}