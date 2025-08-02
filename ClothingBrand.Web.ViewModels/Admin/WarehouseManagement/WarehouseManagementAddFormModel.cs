using System.ComponentModel.DataAnnotations;

using static ClothingBrand.Data.Common.EntityConstants.Warehouse;
using static ClothingBrandApp.Web.ViewModels.ValidationMessages.Warehouse;

namespace ClothingBrandApp.Web.ViewModels.Admin.WarehouseManagement
{
    public class WarehouseManagementAddFormModel
    {
        [Required(ErrorMessage = WarehouseNameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = WarehouseNameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = WarehouseNameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = WarehouseLocationRequiredMessage)]
        [MinLength(LocationMinLength, ErrorMessage = WarehouseLocationMinLengthMessage)]
        [MaxLength(LocationMaxLength, ErrorMessage = WarehouseLocationMaxLengthMessage)]
        public string Location { get; set; } = null!;

        public IEnumerable<string>? AppManagerEmails { get; set; }

        [Required]
        public string ManagerEmail { get; set; } = null!;
    }
}