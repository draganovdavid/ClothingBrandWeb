using ClothingBrand.Data.Models;

namespace ClothingBrand.Data.Repository.Interfaces
{
    public interface IManagerRepository : 
        IRepository<Manager, Guid>, IAsyncRepository<Manager, Guid>
    {
    }
}
