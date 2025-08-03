using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.WarehouseManagement;

namespace ClothingBrand.Services.Core.Admin.Interfaces
{
    public interface IWarehouseManagementService : IWarehouseService
    {
        Task<IEnumerable<WarehouseManagementIndexViewModel>> GetWarehouseManagementBoardDataAsync();

        Task<bool> AddWarehouseAsync(WarehouseManagementAddFormModel? inputModel);

        Task<WarehouseManagementEditFormModel?> GetWarehouseEditFormModelAsync(string? id);

        Task<bool> EditWarehouseAsync(WarehouseManagementEditFormModel? inputModel);

        Task<Tuple<bool, bool>> DeleteOrRestoreWarehouseAsync(string? id);
    }
}