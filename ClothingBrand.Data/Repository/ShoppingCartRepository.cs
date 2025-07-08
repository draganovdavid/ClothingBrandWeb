using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrandApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Data.Repository
{
    public class ShoppingCartRepository : BaseRepository<ApplicationUserShoppingCart, object>, IShoppingCartRepository
    {
        public ShoppingCartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ApplicationUserShoppingCart? GetByCompositeKey(string userId, string productId)
        {
            return this
                .GetAllAttached()
                .SingleOrDefault(aup => aup.ApplicationUserId.ToLower() == userId.ToLower() &&
                                        aup.ProductId.ToString().ToLower() == productId.ToLower());
        }

        public async Task<ApplicationUserShoppingCart?> GetByCompositeKeyAsync(string userId, string productId)
        {
            return await this
               .GetAllAttached()
               .SingleOrDefaultAsync(aup => aup.ApplicationUserId.ToLower() == userId.ToLower() &&
                                       aup.ProductId.ToString().ToLower() == productId.ToLower());
        }

        public bool Exists(string userId, string productId)
        {
            return this
                .GetAllAttached()
                .Any(aup => aup.ApplicationUserId.ToLower() == userId.ToLower() &&
                                        aup.ProductId.ToString().ToLower() == productId.ToLower());
        }

        public Task<bool> ExistsAsync(string userId, string productId)
        {
            return this
                .GetAllAttached()
                .AnyAsync(aup => aup.ApplicationUserId.ToLower() == userId.ToLower() &&
                                        aup.ProductId.ToString().ToLower() == productId.ToLower());
        }
    }
}
