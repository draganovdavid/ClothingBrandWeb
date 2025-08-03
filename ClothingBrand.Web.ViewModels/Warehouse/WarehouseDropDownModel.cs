using System.ComponentModel.DataAnnotations;

namespace ClothingBrandApp.Web.ViewModels.Warehouse
{
    public class WarehouseDropDownModel
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
