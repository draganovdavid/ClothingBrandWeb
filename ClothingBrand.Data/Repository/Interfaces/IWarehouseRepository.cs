using ClothingBrand.Data.Models;

namespace ClothingBrand.Data.Repository.Interfaces
{
    public interface IWarehouseRepository : IRepository<Warehouse, Guid>
        , IAsyncRepository<Warehouse, Guid>
    {
    }
}