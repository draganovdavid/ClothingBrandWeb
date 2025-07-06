using ClothingBrand.Data.Models;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Data;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Services.Core
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public ShoppingCartService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<ProductIndexViewModel>?> GetAllProductsInShoppingCartAsync(string userId)
        {
            IEnumerable<ProductIndexViewModel>? allProductsInShoppingCart = null;

            IdentityUser? user = await this.userManager
                .FindByIdAsync(userId);
            if (user != null)
            {
                allProductsInShoppingCart = await this.dbContext
                .ApplicationUserShoppingCarts
                .AsNoTracking()
                .Select(ausc => new ProductIndexViewModel()
                {
                    Id = ausc.ProductId,
                    Name = ausc.Product.Name,
                    Price = ausc.Product.Price,
                    ImageUrl = ausc.Product.ImageUrl
                })
                .ToArrayAsync();
            }

            return allProductsInShoppingCart;
        }

        public async Task<bool> AddProductToShoppingCartAsync(Guid? productId, string userId)
        {
            if (productId != null)
            {
                var entry = await this.dbContext.ApplicationUserShoppingCarts
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(aup => aup.ApplicationUserId == userId && aup.ProductId == productId);

                if (entry != null)
                {
                    entry.IsDeleted = false;
                    this.dbContext.Update(entry);
                }
                else
                {
                    await this.dbContext.ApplicationUserShoppingCarts.AddAsync(new ApplicationUserShoppingCart
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

        public async Task<bool> DeleteProductFromShoppingCartAsync(Guid? productId, string userId)
        {
            if (productId != null)
            {
                ApplicationUserShoppingCart? userShoppingCartProductEntry = await this.dbContext
                    .ApplicationUserShoppingCarts
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(ausc => ausc.ProductId == productId &&
                                            ausc.ApplicationUserId == userId);

                if (userShoppingCartProductEntry != null)
                {
                    userShoppingCartProductEntry.IsDeleted = true;

                    this.dbContext.ApplicationUserShoppingCarts
                        .Update(userShoppingCartProductEntry);
                    await this.dbContext.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsProductAddedToShoppingCart(Guid? productId, string userId)
        {
            if (productId != null)
            {
                ApplicationUserShoppingCart? userProductEntry = await this.dbContext
                    .ApplicationUserShoppingCarts
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
