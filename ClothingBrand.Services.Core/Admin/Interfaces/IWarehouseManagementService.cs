using ClothingBrandApp.Web.ViewModels.Admin.WarehouseManagement;

namespace ClothingBrand.Services.Core.Admin.Interfaces
{
    public interface IWarehouseManagementService
    {
        Task<IEnumerable<WarehouseManagementIndexViewModel>> GetWarehouseManagementBoardDataAsync();

    }
}