using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Admin.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.WarehouseManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Services.Core.Admin
{
    public class WarehouseManagementService : IWarehouseManagementService
    {
        private readonly IWarehouseRepository warehouseRepository;
        private readonly IManagerRepository managerRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public WarehouseManagementService(IWarehouseRepository warehouseRepository, IManagerRepository managerRepository, UserManager<ApplicationUser> userManager)
        {
            this.warehouseRepository = warehouseRepository;
            this.managerRepository = managerRepository;
            this.userManager = userManager;
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
                    IsDeleted = w.IsDeleted,
                    ManagerName = w.Manager != null ?
                        w.Manager.User.UserName : null
                })
                .ToListAsync();

            return allWarehouses;
        }

        public async Task<bool> AddWarehouseAsync(WarehouseManagementAddFormModel? inputModel)
        {
            bool result = false;
            if (inputModel != null)
            {
                ApplicationUser? managerUser = await this.userManager
                    .FindByNameAsync(inputModel.ManagerEmail);
                if (managerUser != null)
                {
                    Manager? manager = await this.managerRepository
                        .GetAllAttached()
                        .SingleOrDefaultAsync(m => m.UserId.ToLower().ToString() == managerUser.Id.ToLower().ToString());

                    if (manager != null)
                    {
                        Warehouse? newWarehouse = new Warehouse()
                        {
                            Name = inputModel.Name,
                            Location = inputModel.Location,
                            ManagerId = manager.Id,
                        };

                        await this.warehouseRepository.AddAsync(newWarehouse);

                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
