using ClothingBrand.Data.Models;

namespace ClothingBrand.Data.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, int>
        , IAsyncRepository<Category, int>
    {

    }
}
