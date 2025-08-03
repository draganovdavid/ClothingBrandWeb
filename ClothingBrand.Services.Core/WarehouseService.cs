using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Warehouse;
using Microsoft.EntityFrameworkCore;

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
                    ManagerId = w.ManagerId.ToString(),
                })
                .ToListAsync();

            return allWarehouses;
        }

        public async Task<WarehouseStockViewModel?> GetWarehouseProductsAsync(string? id)
        {
            WarehouseStockViewModel? warehouseProducts = null;
            if (!String.IsNullOrWhiteSpace(id))
            {
                Warehouse? warehouse = await warehouseRepository
                    .GetAllAttached()
                    .Where(w => w.Id.ToString().ToLower() == id.ToLower())
                    .Include(w => w.Manager)
                        .ThenInclude(m => m!.User)
                    .Include(w => w.WarehouseProducts)
                        .ThenInclude(p => p.Gender)
                    .FirstOrDefaultAsync();

                if (warehouse != null)
                {
                    warehouseProducts = new WarehouseStockViewModel
                    {
                        WarehouseId = warehouse.Id.ToString(),
                        WarehouseName = $"{warehouse.Name} - {warehouse.Location}",
                        WarehouseProducts = warehouse.WarehouseProducts
                            .Where(p => !p.IsDeleted)
                            .Select(p => new WarehouseStockProductViewModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Price = p.Price,
                                InStock = p.InStock,
                                ImageUrl = p.ImageUrl ?? "/images/NoImage.png",
                                Gender = p.Gender.Name // ✅ safe if .Include(Gender) worked
                            })
                            .ToList()
                    };
                }
            }

            return warehouseProducts;
        }

        public async Task<IEnumerable<WarehouseDropDownModel>> GetAllWarehousesDropDownAsync()
        {
            return await this.warehouseRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(w => new WarehouseDropDownModel()
                {
                    Name = w.Name
                })
                .ToListAsync();
        }
    }
}