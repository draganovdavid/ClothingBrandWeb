using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Admin.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Services.Core.Admin
{
    public class ProductManagementService : ShopService, IProductManagementService
    {
        private readonly IShopRepository shopRepository;
        public ProductManagementService(IShopRepository shopRepository, ICategoryRepository categoryRepository, IWarehouseRepository warehouseRepository) 
            : base(shopRepository, categoryRepository, warehouseRepository)
        {
            this.shopRepository = shopRepository;
        }

        public async Task<IEnumerable<ProductManagementIndexViewModel>> GetProductManagementBoardDataAsync()
        {
            IEnumerable<ProductManagementIndexViewModel> allProducts = await this.shopRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Select(p => new ProductManagementIndexViewModel()
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    Size = p.Size,
                    Description = p.Description,
                    Gender = p.Gender.Name,
                    Category = p.Category.Name,
                    InStock = p.InStock,
                    IsDeleted = p.IsDeleted,
                })
                .ToListAsync();

            return allProducts;
        }
    }
}
