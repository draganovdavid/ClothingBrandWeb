using ClothingBrandApp.Web.ViewModels.Warehouse;

namespace ClothingBrand.Services.Core.Interfaces
{
    public interface IWarehouseService
    {
        Task<IEnumerable<WarehouseIndexViewModel>> GetAllWarehousesViewAsync();

        Task<WarehouseStockViewModel?> GetWarehouseProductsAsync(string? id);

        Task<IEnumerable<WarehouseDropDownModel>> GetAllWarehousesDropDownAsync();
    }
}