using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Warehouse;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrandApp.Web.Controllers
{
    public class WarehouseController : BaseController
    {
        private readonly IWarehouseService warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            this.warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<WarehouseIndexViewModel> allWarehouses = await this.warehouseService
                .GetAllWarehousesViewAsync();

                return View(allWarehouses);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Stock(string? id)
        {
            try
            {
                WarehouseStockViewModel? clothingWarehouses = await this.warehouseService
                    .GetWarehouseProductsAsync(id);
                if (clothingWarehouses == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(clothingWarehouses);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}