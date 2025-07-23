using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Warehouse;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ClothingBrand.Services.Core
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            this.warehouseRepository = warehouseRepository;
        }

        public async Task<IEnumerable<WarehouseIndexViewModel>> GetAllWarehousesViewAsync()
        {
            IEnumerable<WarehouseIndexViewModel> allWarehouses = await this.warehouseRepository
                .GetAllAttached()
                .Select(w => new WarehouseIndexViewModel()
                {
                    Id = w.Id.ToString(),
                    Name = w.Name,
                    Location = w.Location,
                })
                .ToListAsync();

            return allWarehouses;
        }
    }
}