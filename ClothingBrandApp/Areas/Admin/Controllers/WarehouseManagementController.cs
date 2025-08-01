using ClothingBrand.Services.Core.Admin.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.WarehouseManagement;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrandApp.Web.Areas.Admin.Controllers
{
    public class WarehouseManagementController : BaseAdminController
    {
        private readonly IWarehouseManagementService warehouseManagementService;

        public WarehouseManagementController(IWarehouseManagementService warehouseManagementService)
        {
            this.warehouseManagementService = warehouseManagementService;
        }

        public async Task<IActionResult> Manage()
        {
            IEnumerable<WarehouseManagementIndexViewModel> allWarehouses = await this.warehouseManagementService
                .GetWarehouseManagementBoardDataAsync();

            return View(allWarehouses);
        }
    }
}
