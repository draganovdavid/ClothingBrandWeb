using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrandApp.Data;

namespace ClothingBrand.Data.Repository
{
    public class ManagerRepository : BaseRepository<Manager, Guid>, IManagerRepository
    {
        public ManagerRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}