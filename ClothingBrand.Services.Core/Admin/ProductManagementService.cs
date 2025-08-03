using ClothingBrand.Data.Models;
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

        public async Task<Tuple<bool, bool>> DeleteOrRestoreProductAsync(string? id)
        {
            bool result = false;
            bool isRestored = false;
            if (!String.IsNullOrWhiteSpace(id))
            {
                Product? product = await this.shopRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(p => p.Id.ToString().ToLower() == id.ToLower());

                if (product != null)
                {
                    if (product.IsDeleted)
                    {
                        isRestored = true;
                    }
                    product.IsDeleted = !product.IsDeleted;

                    result = await this.shopRepository.UpdateAsync(product);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
        }
    }
}
