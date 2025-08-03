using ClothingBrandApp.Web.ViewModels.Admin.WarehouseManagement;

namespace ClothingBrand.Services.Core.Admin.Interfaces
{
    public interface IWarehouseManagementService
    {
        Task<IEnumerable<WarehouseManagementIndexViewModel>> GetWarehouseManagementBoardDataAsync();

        Task<bool> AddWarehouseAsync(WarehouseManagementAddFormModel? inputModel);

        Task<WarehouseManagementEditFormModel?> GetWarehouseEditFormModelAsync(string? id);

        Task<bool> EditWarehouseAsync(WarehouseManagementEditFormModel? inputModel);

    }
}