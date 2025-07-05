namespace ClothingBrandApp.Web.ViewModels.Product
{
    public class ProductDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Size { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool InStock { get; set; }
        public string CategoryName { get; set; } = null!;
        public string GenderName { get; set; } = null!;
        public bool IsFavorite { get; set; }
    }
}
