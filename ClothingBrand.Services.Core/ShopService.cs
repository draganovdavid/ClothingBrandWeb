using ClothingBrand.Data.Models;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Data;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                .Select(p => new ProductIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    InStock = p.InStock
                })
                .ToListAsync();
        }

        public async Task<bool> AddProductAsync(string userId, ProductFormInputModel inputModel)
        {
            IdentityUser? user = await userManager.FindByIdAsync(userId);
            Category? category = await this.dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == inputModel.CategoryId);
            Gender? gender = await this.dbContext.Genders
                .FirstOrDefaultAsync(g => g.Name.ToLower() == inputModel.Gender.ToLower());

            if (user != null && category != null && gender != null && inputModel.Price.HasValue)
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = inputModel.Name,
                    Price = inputModel.Price.Value,
                    Description = inputModel.Description,
                    Size = inputModel.Size,
                    ImageUrl = inputModel.ImageUrl,
                    InStock = inputModel.InStock,
                    AuthorId = user.Id,
                    CategoryId = category.Id,
                    GenderId = gender.Id,
                    IsDeleted = false
                };

                await this.dbContext.Products.AddAsync(product);
                await this.dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<ProductDetailsViewModel?> GetProductDetailsByIdAsync(Guid? id)
        {
            return await this.dbContext.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Include(p => p.Category)
                .Include(p => p.Gender)
                .Select(p => new ProductDetailsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Size = p.Size,
                    ImageUrl = p.ImageUrl,
                    InStock = p.InStock,
                    CategoryName = p.Category.Name,
                    GenderName = p.Gender.Name,
                })
                .SingleOrDefaultAsync();
        }

        public async Task<ProductFormInputModel?> GetProductForEditingAsync(Guid? productId)
        {
            if (productId == null)
                return null;

            return await this.dbContext.Products
                .AsNoTracking()
                .Where(p => p.Id == productId)
                .Select(p => new ProductFormInputModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    Gender = p.Gender.Name,
                    Size = p.Size,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    InStock = p.InStock
                })
                .SingleOrDefaultAsync();
        }

        public async Task<bool> EditProductAsync(string userId, ProductFormInputModel inputModel)
        {
            if (!Guid.TryParse(inputModel.Id, out Guid productId))
            {
                return false;
            }
            Product? updatedProduct = await this.dbContext.Products
                .FindAsync(productId);

            Category? category = await this.dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == inputModel.CategoryId);
            Gender? gender = await this.dbContext.Genders
                .FirstOrDefaultAsync(g => g.Name.ToLower() == inputModel.Gender.ToLower());

            if (userId != null && updatedProduct != null && category != null && gender != null && inputModel.Price.HasValue)
            {
                updatedProduct.Name = inputModel.Name;
                updatedProduct.Price = inputModel.Price.Value;
                updatedProduct.Description = inputModel.Description;
                updatedProduct.Size = inputModel.Size;
                updatedProduct.CategoryId = category.Id;
                updatedProduct.GenderId = gender.Id;
                updatedProduct.ImageUrl = inputModel.ImageUrl;
                updatedProduct.InStock = inputModel.InStock;

                await this.dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<ProductDeleteInputModel?> GetProductForDeleteAsync(Guid? productId)
        {
            ProductDeleteInputModel? deleteModel = null;

            if (productId != null)
            {
                Product? deleteProductModel = await this.dbContext.Products
                    .AsNoTracking()
                    .SingleOrDefaultAsync(p => p.Id == productId);

                if (deleteProductModel != null)
                {
                    deleteModel = new ProductDeleteInputModel()
                    {
                        Id = deleteProductModel.Id,
                        Name = deleteProductModel.Name,
                        ImageUrl = deleteProductModel.ImageUrl,
                    };
                }
            }

            return deleteModel;
        }

        public async Task<bool> SoftDeleteAsync(ProductDeleteInputModel inputModel)
        {
            Product? deletedProduct = await this.dbContext.Products
                .FindAsync(inputModel.Id);

            if (deletedProduct != null)
            {
                deletedProduct.IsDeleted = true;
                await this.dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }


        public async Task<IEnumerable<ProductIndexViewModel>> GetProductsByGenderAsync(string genderName)
        {
            var allProductsForMen = await this.dbContext.Products
                .AsNoTracking()
                .Where(p => p.Gender.Name == genderName)
                .Select(p => new ProductIndexViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    InStock = p.InStock
                })
                .ToListAsync();

            return allProductsForMen;
        }
    }
}