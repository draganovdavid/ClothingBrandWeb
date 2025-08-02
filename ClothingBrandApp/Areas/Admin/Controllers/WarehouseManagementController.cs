using ClothingBrand.Services.Core.Admin.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.WarehouseManagement;
using Microsoft.AspNetCore.Mvc;

using static ClothingBrandApp.GCommon.ApplicationConstants;

namespace ClothingBrandApp.Web.Areas.Admin.Controllers
{
    public class WarehouseManagementController : BaseAdminController
    {
        private readonly IWarehouseManagementService warehouseManagementService;
        private readonly IUserService userService;

        public WarehouseManagementController(IWarehouseManagementService warehouseManagementService, IUserService userService)
        {
            this.warehouseManagementService = warehouseManagementService;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            IEnumerable<WarehouseManagementIndexViewModel> allWarehouses = await this.warehouseManagementService
                .GetWarehouseManagementBoardDataAsync();

            return View(allWarehouses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            WarehouseManagementAddFormModel viewModel = new WarehouseManagementAddFormModel()
            {
                AppManagerEmails = await this.userService.GetManagerEmailsAsync()
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WarehouseManagementAddFormModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                bool success = await this.warehouseManagementService
                    .AddWarehouseAsync(inputModel);

                if (!success)
                {
                    TempData[ErrorMessageKey] = "Error occurred while adding the warehouse! Ensure to select a valid manager!";
                }
                else
                {
                    TempData[SuccessMessageKey] = "Warehouse created successfully!";
                }

                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Unexpected error occurred while adding the warehouse! Please contact developer team!";
                return this.RedirectToAction(nameof(Manage));
            }
        }
    }
}