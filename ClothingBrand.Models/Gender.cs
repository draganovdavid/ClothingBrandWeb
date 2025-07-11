using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Data.Models
{
    public class Gender
    {
        [Comment("Gender identifier")]
        public int Id { get; set; }

        [Comment("Gender name")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } 
            = new HashSet<Product>();
    }
}