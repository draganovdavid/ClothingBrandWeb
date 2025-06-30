namespace ClothingBrand.Data.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Size { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool InStock { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int GenderId { get; set; }
        public Gender Gender { get; set; } = null!;

        public virtual ICollection<ApplicationUserShoppingCart> UserShoppingCartItems { get; set; } 
            = new HashSet<ApplicationUserShoppingCart>();
        public virtual ICollection<ApplicationUserProduct> UserFavorites { get; set; }
            = new HashSet<ApplicationUserProduct>();

        public bool IsDeleted { get; set; }
    }
}
