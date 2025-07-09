using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Services.Core
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            this.favoriteRepository = favoriteRepository;
        }

        public async Task<IEnumerable<ProductIndexViewModel>?> GetUserFavoriteProductsAsync(string userId)
        {
            IEnumerable<ProductIndexViewModel>? favProducts = null;

            favProducts = await this.favoriteRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(aup => aup.ApplicationUserId == userId)
                .Select(aup => new ProductIndexViewModel()
                {
                    Id = aup.ProductId,
                    Name = aup.Product.Name,
                    Price = aup.Product.Price,
                    ImageUrl = aup.Product.ImageUrl
                })
                .ToListAsync();

            return favProducts;
        }

        public async Task<bool> AddProductToUserFavoritesAsync(Guid? productId, string userId)
        {
            if (productId == null)
            {
                return false;
            }

            ApplicationUserProduct? userProductEntry = await this.favoriteRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(aup => aup.ApplicationUserId == userId && 
                                        aup.ProductId == productId);

            if (userProductEntry != null)
            {
                userProductEntry.IsDeleted = false;
                return await this.favoriteRepository.UpdateAsync(userProductEntry);
            }
            else
            {
                userProductEntry = new ApplicationUserProduct()
                {
                    ApplicationUserId = userId,
                    ProductId = productId.Value
                };

                await this.favoriteRepository.AddAsync(userProductEntry);
                return true;
            }
        }

        public async Task<bool> DeleteProductFromUserFavoritesAsync(Guid? productId, string? userId)
        {
            if (productId != null && userId != null)
            {
                ApplicationUserProduct? userProductEntry = await this.favoriteRepository
                    .SingleOrDefaultAsync(upe => upe.ApplicationUserId == userId
                        && upe.ProductId == productId);

                if (userProductEntry != null)
                {
                    userProductEntry.IsDeleted = true;

                    return await this.favoriteRepository.DeleteAsync(userProductEntry);
                }
            }

            return false;
        }

        public async Task<bool> IsProductAddedToFavorites(Guid? productId, string userId)
        {
            if (productId != null && userId != null)
            {
                ApplicationUserProduct? userProductEntry = await this.favoriteRepository
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