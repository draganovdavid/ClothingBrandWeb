using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using static ClothingBrandApp.GCommon.ApplicationConstants;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository shoppingCartRepository;

    public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
    {
        this.shoppingCartRepository = shoppingCartRepository;
    }

    public async Task<IEnumerable<ProductIndexViewModel>> GetAllProductsInShoppingCartAsync(string userId)
    {
        IEnumerable<ProductIndexViewModel> allShopCartProd = await this.shoppingCartRepository
            .GetAllAttached()
            .Include(ausc => ausc.Product)
            .AsNoTracking()
            .Where(ausc => ausc.ApplicationUserId == userId)
            .Select(ausc => new ProductIndexViewModel
            {
                Id = ausc.ProductId,
                Name = ausc.Product.Name,
                Price = ausc.Product.Price,
                ImageUrl = ausc.Product.ImageUrl
            })
            .ToListAsync();
        foreach (ProductIndexViewModel product in allShopCartProd)
        {
            if (String.IsNullOrEmpty(product.ImageUrl))
            {
                product.ImageUrl = $"/images/{NoImageUrl}";
            }
        }

        return allShopCartProd;
    }

    public async Task<bool> AddProductToShoppingCartAsync(Guid? productId, string userId)
    {
        if (!productId.HasValue)
        {
            return false;
        }

        ApplicationUserShoppingCart? userShopppingCartEntry = await this.shoppingCartRepository
            .GetAllAttached()
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(aup => aup.ApplicationUserId == userId 
                                            && aup.ProductId == productId);

        if (userShopppingCartEntry != null)
        {
            userShopppingCartEntry.IsDeleted = false;
            return await this.shoppingCartRepository.UpdateAsync(userShopppingCartEntry);
        }
        else
        {
            userShopppingCartEntry = new ApplicationUserShoppingCart()
            {
                ApplicationUserId = userId,
                ProductId = productId.Value
            };
            await this.shoppingCartRepository.AddAsync(userShopppingCartEntry);

            return true;
        }
    }

    public async Task<bool> DeleteProductFromShoppingCartAsync(Guid? productId, string userId)
    {
        if (!productId.HasValue) 
        {
            return false;
        }

        ApplicationUserShoppingCart? userShoppingCartEntry = await this.shoppingCartRepository
            .GetAllAttached()
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(ausc => ausc.ProductId == productId && 
                                        ausc.ApplicationUserId == userId);

        if (userShoppingCartEntry == null)
        {
            return false;
        }

        userShoppingCartEntry.IsDeleted = true;
        await this.shoppingCartRepository.UpdateAsync(userShoppingCartEntry);
        await this.shoppingCartRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsProductAddedToShoppingCart(Guid? productId, string userId)
    {
        if (!productId.HasValue)
        {
            return false;
        }

        return await this.shoppingCartRepository
            .GetAllAttached()
            .AnyAsync(upe => upe.ApplicationUserId == userId && 
                                     upe.ProductId == productId);
    }
}
