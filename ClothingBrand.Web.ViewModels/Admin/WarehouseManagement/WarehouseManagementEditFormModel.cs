using System.ComponentModel.DataAnnotations;

namespace ClothingBrandApp.Web.ViewModels.Admin.WarehouseManagement
{
    public class WarehouseManagementEditFormModel : WarehouseManagementAddFormModel
    {
        [Required]
        public string Id { get; set; } = null!;
    }
}
