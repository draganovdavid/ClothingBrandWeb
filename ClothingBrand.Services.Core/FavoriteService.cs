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
                    .Include(aup => aup.Product)
                    .ThenInclude(p => p.Category)
                    .Where(aup => aup.ApplicationUserId.ToLower() == userId.ToLower())
                    .Select(aup => new ProductIndexViewModel()
                    {
                        Id = aup.ProductId,
                        Name = aup.Product.Name,
                        ImageUrl = aup.Product.ImageUrl,
                    })
                    .ToArrayAsync();
            }

            return favProducts;
        }

        public async Task AddProductToUserFavoritesAsync(Guid? productId, string userId)
        {
            if (productId != null)
            {
                var entry = await this.dbContext.ApplicationUserProducts
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId && x.ProductId == productId);

                if (entry != null)
                {
                    entry.IsDeleted = false;
                    dbContext.Update(entry);
                }
                else
                {
                    await dbContext.ApplicationUserProducts.AddAsync(new ApplicationUserProduct
                    {
                        ApplicationUserId = userId,
                        ProductId = productId.Value
                    });
                }

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteProductFromUserFavoritesAsync(Guid? productId, string? userId)
        {
            if (productId != null && userId != null)
            {
                ApplicationUserProduct? userProductEntry = await this.dbContext
                    .ApplicationUserProducts
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

        public async Task<bool> IsProductAddedToFavorites(Guid? productId, string? userId)
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