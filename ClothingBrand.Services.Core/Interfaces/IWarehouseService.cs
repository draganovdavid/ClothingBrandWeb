using ClothingBrandApp.Web.ViewModels.Warehouse;

namespace ClothingBrand.Services.Core.Interfaces
{
    public interface IWarehouseService
    {
        Task<IEnumerable<WarehouseIndexViewModel>> GetAllWarehousesViewAsync();


    }
}