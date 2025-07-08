using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrandApp.Data;

namespace ClothingBrand.Data.Repository
{
    public class ShopRepository : BaseRepository<Product, Guid>, IShopRepository
    {
        public ShopRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {

        }
    }
}
