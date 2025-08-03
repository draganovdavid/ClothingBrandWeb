using ClothingBrand.Services.Core.Admin.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.ProductManagement;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrandApp.Web.Areas.Admin.Controllers
{
    public class ProductManagementController : BaseAdminController
    {
        private readonly IProductManagementService productManagementService;

        public ProductManagementController(IProductManagementService productManagementService)
        {
            this.productManagementService = productManagementService;
        }

        public async Task<IActionResult> Manage()
        {
            IEnumerable<ProductManagementIndexViewModel> allProducts = await this.productManagementService
                .GetProductManagementBoardDataAsync();

            return View(allProducts);
        }
    }
}
