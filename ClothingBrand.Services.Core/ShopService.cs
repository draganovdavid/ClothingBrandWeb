using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Data;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Services.Core
{
    public class ShopService : IShopService
    {
        private readonly ApplicationDbContext dbContext;

        public ShopService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync()
        {
            return await dbContext.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Select(p => new ProductIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category.Name,
                    Price = p.Price,
                    InStock = p.InStock
                })
                .ToListAsync();
        }
    }
}
