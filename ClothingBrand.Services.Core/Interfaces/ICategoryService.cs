using ClothingBrandApp.Web.ViewModels.Product;

namespace ClothingBrand.Services.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<AddProductCatgoryDropDownModel>> GetAllCategoriesDropDownAsync();
    }
}
