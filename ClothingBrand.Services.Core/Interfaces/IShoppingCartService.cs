using ClothingBrandApp.Web.ViewModels.Product;

namespace ClothingBrand.Services.Core.Interfaces
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<ProductIndexViewModel>> GetAllProductsInShoppingCartAsync(string userId);

        Task<bool> AddProductToShoppingCartAsync(Guid? productId, string userId);

        Task<bool> DeleteProductFromShoppingCartAsync(Guid? productId, string userId);

        Task<bool> IsProductAddedToShoppingCart(Guid? productId, string userId);
    }
}
