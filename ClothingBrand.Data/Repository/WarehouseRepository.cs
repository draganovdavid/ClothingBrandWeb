using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrandApp.Data;

namespace ClothingBrand.Data.Repository
{
    public class WarehouseRepository : BaseRepository<Warehouse, Guid>, IWarehouseRepository
    {
        public WarehouseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
