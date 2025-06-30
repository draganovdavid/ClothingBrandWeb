using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Data.Models
{
    public class ApplicationUserProduct
    {
        [Comment("Foreign key to the reference Application User.")]
        public string ApplicationUserId { get; set; } = null!;
        public virtual IdentityUser ApplicationUser { get; set; } = null!;

        [Comment("Foreign key to the reference Product.")]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        [Comment("Shows if ApplicationUserProduct entry is deleted")]
        public bool IsDeleted { get; set; }
    }
}
