using ClothingBrandApp.Web.ViewModels.Product;

namespace ClothingBrand.Services.Core.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<ProductIndexViewModel>?> GetUserFavoriteProductsAsync(string userId);

        Task<bool> AddProductToUserFavoritesAsync(Guid? productId, string userId);

        Task<bool> DeleteProductFromUserFavoritesAsync(Guid? productId, string? userId);

        Task<bool> IsProductAddedToFavorites(Guid? productId, string? userId);

    }
}
