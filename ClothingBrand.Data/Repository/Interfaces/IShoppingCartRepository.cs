using ClothingBrand.Data.Models;

namespace ClothingBrand.Data.Repository.Interfaces
{
    public interface IShoppingCartRepository : IRepository<ApplicationUserShoppingCart, object>
        , IAsyncRepository<ApplicationUserShoppingCart, object>
    {
        ApplicationUserShoppingCart? GetByCompositeKey(string userId, string productId);

        Task<ApplicationUserShoppingCart?> GetByCompositeKeyAsync(string userId, string productId);

        bool Exists(string userId, string productId);

        Task<bool> ExistsAsync(string userId, string productId);
    }
}