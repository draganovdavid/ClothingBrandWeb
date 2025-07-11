using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Data.Models
{
    public class Category
    {
        [Comment("Category identifier")]
        public int Id { get; set; }

        [Comment("Category name")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } 
            = new HashSet<Product>();
    }
}
