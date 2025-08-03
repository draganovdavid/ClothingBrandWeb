using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.ProductManagement;

namespace ClothingBrand.Services.Core.Admin.Interfaces
{
    public interface IProductManagementService : IShopService
    {
        Task<IEnumerable<ProductManagementIndexViewModel>> GetProductManagementBoardDataAsync();

        Task<Tuple<bool, bool>> DeleteOrRestoreProductAsync(string? id);
    }
}