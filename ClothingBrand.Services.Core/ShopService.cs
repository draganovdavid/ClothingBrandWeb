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
            return await dbContext
                .Products
                .AsNoTracking()
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

        public async Task<ProductDetailsViewModel?> GetProductDetailsByIdAsync(string? id)
        {
            if (!Guid.TryParse(id, out Guid productId))
                return null;

            return await dbContext.Products
                .AsNoTracking()
                .Where(p => p.Id == productId)
                .Include(p => p.Category)
                .Include(p => p.Gender)
                .Select(p => new ProductDetailsViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Size = p.Size,
                    ImageUrl = p.ImageUrl,
                    InStock = p.InStock,
                    CategoryName = p.Category.Name,
                    GenderName = p.Gender.Name
                })
                .SingleOrDefaultAsync();
        }
    }
}
