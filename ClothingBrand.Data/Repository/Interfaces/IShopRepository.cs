using ClothingBrand.Data.Models;

namespace ClothingBrand.Data.Repository.Interfaces
{
    public interface IShopRepository : IRepository<Product, Guid>
        , IAsyncRepository<Product, Guid>
    {

    }
}