namespace ClothingBrandApp.Web.ViewModels.Product
{
    public class ProductIndexViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }

        public bool InStock { get; set; }
        public bool IsInShoppingCart { get; set; }
        public bool IsFavorite { get; set; }
    }
}
