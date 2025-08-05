using ClothingBrandApp.Web.ViewModels.Category;

namespace ClothingBrand.Services.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<AllCategoriesDropDownViewModel>> GetAllCategoriesDropDownAsync();
    }
}