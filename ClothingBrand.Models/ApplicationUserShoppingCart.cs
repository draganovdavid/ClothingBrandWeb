using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Data.Models
{
    [Comment("User's products in shopping cart")]
    public class ApplicationUserShoppingCart
    {
        [Comment("Foreign key to the reference Application User.")]
        public string ApplicationUserId { get; set; } = null!;
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [Comment("Foreign key to the reference Item in Shopping Cart.")]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        [Comment("Shows if ApplicationUserShoppingCart entry is deleted")]
        public bool IsDeleted { get; set; }
    }
}