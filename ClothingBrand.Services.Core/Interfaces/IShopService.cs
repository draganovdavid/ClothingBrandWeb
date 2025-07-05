using ClothingBrandApp.Web.ViewModels.Product;

namespace ClothingBrand.Services.Core.Interfaces
{
    public interface IShopService
    {
        Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync();

        Task<bool> AddProductAsync(string userId, ProductFormInputModel inputModel);

        Task<ProductDetailsViewModel?> GetProductDetailsByIdAsync(Guid? id);

        Task<ProductFormInputModel?> GetProductForEditingAsync(Guid? productId);

        Task<bool> EditProductAsync(string userId, ProductFormInputModel inputModel);

        Task<ProductDeleteInputModel?> GetProductForDeleteAsync(Guid? productId);

        Task<bool> SoftDeleteAsync(ProductDeleteInputModel inputModel);

    }
}
