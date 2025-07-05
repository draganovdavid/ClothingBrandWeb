namespace ClothingBrandApp.Web.ViewModels.Product
{
    public class ProductIndexViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }

        // Used for the favorite heart icon in the view
        public bool IsFavorite { get; set; }
    }
}
