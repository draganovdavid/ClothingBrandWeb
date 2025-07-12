using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Data.Models
{
    public class Product
    {
        [Comment("Product identifier")]
        public Guid Id { get; set; }

        [Comment("Product name")]
        public string Name { get; set; } = null!;

        [Comment("Product description")]
        public string Description { get; set; } = null!;

        [Comment("Product price")]
        public decimal Price { get; set; }

        [Comment("Product size")]
        public string Size { get; set; } = null!;

        [Comment("Product ImageUrl")]
        public string? ImageUrl { get; set; }

        [Comment("Product availability")]
        public bool InStock { get; set; }

        [Comment("Product warehouse identifier")]
        public Guid WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; } = null!;

        [Comment("Product category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Comment("Product gender")]
        public int GenderId { get; set; }
        public Gender Gender { get; set; } = null!;

        [Comment("Shows if product is deleted")]
        public bool IsDeleted { get; set; }

        public virtual ICollection<ApplicationUserShoppingCart> UserShoppingCartItems { get; set; } 
            = new HashSet<ApplicationUserShoppingCart>();
        public virtual ICollection<ApplicationUserProduct> UserFavorites { get; set; }
            = new HashSet<ApplicationUserProduct>();
    }
}
