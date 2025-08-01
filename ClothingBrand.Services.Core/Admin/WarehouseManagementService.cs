using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Admin.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.WarehouseManagement;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Services.Core.Admin
{
    public class WarehouseManagementService : IWarehouseManagementService
    {
        private readonly IWarehouseRepository warehouseRepository;

        public WarehouseManagementService(IWarehouseRepository warehouseRepository)
        {
            this.warehouseRepository = warehouseRepository;
        }

        public async Task<IEnumerable<WarehouseManagementIndexViewModel>> GetWarehouseManagementBoardDataAsync()
        {
            IEnumerable<WarehouseManagementIndexViewModel> allWarehouses = await this.warehouseRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Select(w => new WarehouseManagementIndexViewModel()
                {
                    Id = w.Id.ToString(),
                    Name = w.Name,
                    Location = w.Location,
                    IsDeleted = w.IsDeleted
                })
                .ToListAsync();

            return allWarehouses;

        }
    }
}
