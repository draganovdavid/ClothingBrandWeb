using System.ComponentModel.DataAnnotations;
using static ClothingBrand.Data.Common.EntityConstants.Product;
using static ClothingBrandApp.Web.ViewModels.ValidationMessages.Product;

namespace ClothingBrandApp.Web.ViewModels.Product
{
    public class ProductFormInputModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = NameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = CategoryRequiredMessage)]
        public int CategoryId { get; set; }
        public IEnumerable<AddProductCatgoryDropDownModel>? Categories { get; set; }

        [Required(ErrorMessage = GenderRequiredMessage)]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = SizeRequiredMessage)]
        [MinLength(SizeMinLength, ErrorMessage = SizeMinLengthMessage)]
        [MaxLength(SizeMaxLength, ErrorMessage = SizeMaxLengthMessage)]
        public string Size { get; set; } = null!;

        [Required(ErrorMessage = PriceRequiredMessage)]
        [Range(0.01, double.MaxValue, ErrorMessage = PriceRangeMessage)]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string Description { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthMessage)]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = InStockRequiredMessage)]
        public bool InStock { get; set; }
    }
}
