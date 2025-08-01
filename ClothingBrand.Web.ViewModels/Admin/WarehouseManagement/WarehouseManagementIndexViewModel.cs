namespace ClothingBrandApp.Web.ViewModels.Admin.WarehouseManagement
{
    public class WarehouseManagementIndexViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Location { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public string? ManagerName { get; set; }
    }
}