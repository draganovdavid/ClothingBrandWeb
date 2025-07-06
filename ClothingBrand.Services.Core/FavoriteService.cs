using ClothingBrand.Data.Models;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Data;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Services.Core
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public FavoriteService(ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<ProductIndexViewModel>?> GetUserFavoriteProductsAsync(string userId)
        {
            IEnumerable<ProductIndexViewModel>? favProducts = null;

            IdentityUser? user = await this.userManager
                .FindByIdAsync(userId);

            if (user != null)
            {
                favProducts = await this.dbContext
                    .ApplicationUserProducts
                    .AsNoTracking()
                    .Where(aup => aup.ApplicationUserId == userId)
                    .Select(aup => new ProductIndexViewModel()
                    {
                        Id = aup.ProductId,
                        Name = aup.Product.Name,
                        Price = aup.Product.Price,
                        ImageUrl = aup.Product.ImageUrl
                    })
                    .ToArrayAsync();
            }

            return favProducts;
        }

        public async Task<bool> AddProductToUserFavoritesAsync(Guid? productId, string userId)
        {
            if (productId != null)
            {
                var entry = await this.dbContext.ApplicationUserProducts
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(aup => aup.ApplicationUserId == userId && aup.ProductId == productId);

                if (entry != null)
                {
                    entry.IsDeleted = false;
                    this.dbContext.Update(entry);
                }
                else
                {
                    await this.dbContext.ApplicationUserProducts.AddAsync(new ApplicationUserProduct
                    {
                        ApplicationUserId = userId,
                        ProductId = productId.Value
                    });
                }

                await this.dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }


        public async Task<bool> DeleteProductFromUserFavoritesAsync(Guid? productId, string? userId)
        {
            if (productId != null && userId != null)
            {
                ApplicationUserProduct? userProductEntry = await this.dbContext
                    .ApplicationUserProducts
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(upe => upe.ApplicationUserId == userId
                        && upe.ProductId == productId);

                if (userProductEntry != null)
                {
                    userProductEntry.IsDeleted = true;

                    this.dbContext.ApplicationUserProducts.Update(userProductEntry);
                    await this.dbContext.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsProductAddedToFavorites(Guid? productId, string userId)
        {
            if (productId != null && userId != null)
            {
                ApplicationUserProduct? userProductEntry = await this.dbContext
                    .ApplicationUserProducts
                    .SingleOrDefaultAsync(upe => upe.ApplicationUserId == userId
                    && upe.ProductId == productId);

                if (userProductEntry != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}