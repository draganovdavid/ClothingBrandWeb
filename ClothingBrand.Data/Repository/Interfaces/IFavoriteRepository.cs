using ClothingBrand.Data.Models;

namespace ClothingBrand.Data.Repository.Interfaces
{
    public interface IFavoriteRepository : IRepository<ApplicationUserProduct, object>
        , IAsyncRepository<ApplicationUserProduct, object>
    {
        ApplicationUserProduct? GetByCompositeKey(string userId, string productId);

        Task<ApplicationUserProduct?> GetByCompositeKeyAsync(string userId, string productId);

        bool Exists(string userId, string productId);

        Task<bool> ExistsAsync(string userId, string productId);
    }
}