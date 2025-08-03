using ClothingBrand.Data.Models;
using ClothingBrandApp.Web.ViewModels.Product;

namespace ClothingBrand.Services.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesDropDownAsync();
    }
}
