using ClothingBrand.Data.Models;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Data;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ClothingBrand.Services.Core
{
    public class ShopService : IShopService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public ShopService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync()
        {
            return await dbContext
                .Products
                .AsNoTracking()
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

        public async Task<bool> AddProductAsync(string userId, ProductFormInputModel inputModel)
        {
            bool result = false;
            IdentityUser? user = await this.userManager.FindByIdAsync(userId);
            Category? categoryRef = await this.dbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == inputModel.CategoryId);

            int genderId = 0;
            if (!string.IsNullOrWhiteSpace(inputModel.Gender))
            {
                var gender = dbContext.Genders
                    .AsNoTracking()
                    .FirstOrDefault(g => g.Name.ToLower() == inputModel.Gender.ToLower());

                genderId = gender?.Id ?? 0;
            }


            if (user != null && categoryRef != null && genderId != 0 && inputModel.Price.HasValue)
            {
                Product product = new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = inputModel.Name,
                    Price = inputModel.Price.Value,
                    Description = inputModel.Description,
                    Size = inputModel.Size,
                    ImageUrl = inputModel.ImageUrl,
                    InStock = inputModel.InStock,
                    AuthorId = user.Id,
                    CategoryId = categoryRef.Id,
                    GenderId = genderId,
                    IsDeleted = false
                };

                await this.dbContext.Products.AddAsync(product);
                await this.dbContext.SaveChangesAsync();

                result = true;
            }

            return result;
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
