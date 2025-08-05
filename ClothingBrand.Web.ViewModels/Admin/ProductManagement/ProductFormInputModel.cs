using ClothingBrandApp.Web.ViewModels.Category;
using ClothingBrandApp.Web.ViewModels.Warehouse;
using System.ComponentModel.DataAnnotations;
using static ClothingBrand.Data.Common.EntityConstants.Product;
using static ClothingBrandApp.Web.ViewModels.ValidationMessages.Product;
using static ClothingBrandApp.Web.ViewModels.ValidationMessages.Warehouse;

namespace ClothingBrandApp.Web.ViewModels.Admin.ProductManagement
{
    public class ProductFormInputModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = NameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = CategoryRequiredMessage)]
        public string CategoryName { get; set; } = null!;
        public IEnumerable<AllCategoriesDropDownViewModel>? Categories { get; set; }

        [Required(ErrorMessage = GenderRequiredMessage)]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = SizeRequiredMessage)]
        [MinLength(SizeMinLength, ErrorMessage = SizeMinLengthMessage)]
        [MaxLength(SizeMaxLength, ErrorMessage = SizeMaxLengthMessage)]
        public string Size { get; set; } = null!;

        [Required(ErrorMessage = PriceRequiredMessage)]
        [Range(0.01, double.MaxValue, ErrorMessage = PriceRangeMessage)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = WarehouseNameRequiredMessage)]
        public string WarehouseName { get; set; } = null!;
        public IEnumerable<WarehouseDropDownModel>? Warehouses { get; set; }

        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthMessage)]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = InStockRequiredMessage)]
        public bool InStock { get; set; }
    }
}