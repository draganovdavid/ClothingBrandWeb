using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Data;

namespace ClothingBrand.Services.Core
{
    public class ShopService : IShopService
    {
        private readonly ApplicationDbContext dbContext;

        public ShopService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


    }
}
