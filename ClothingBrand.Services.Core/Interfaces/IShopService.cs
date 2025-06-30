using ClothingBrandApp.Web.ViewModels.Product;

namespace ClothingBrand.Services.Core.Interfaces
{
    public interface IShopService
    {
        Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync();
    }
}
