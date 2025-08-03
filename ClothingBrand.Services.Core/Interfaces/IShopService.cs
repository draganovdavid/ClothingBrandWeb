using ClothingBrandApp.Web.ViewModels.Admin.ProductManagement;
using ClothingBrandApp.Web.ViewModels.Product;

namespace ClothingBrand.Services.Core.Interfaces
{
    public interface IShopService
    {
        Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync();

        Task AddProductAsync(ProductFormInputModel inputModel);

        Task<ProductDetailsViewModel?> GetProductDetailsByIdAsync(Guid? id);

        Task<ProductFormInputModel?> GetProductForEditingAsync(Guid? productId);

        Task<bool> EditProductAsync(ProductFormInputModel inputModel);

        Task<bool> SoftDeleteProductAsync(string? id);

        Task<bool> DeleteProductAsync(string? id);

        Task<IEnumerable<ProductIndexViewModel>> GetProductsByGenderAsync(string genderName);
    }
}