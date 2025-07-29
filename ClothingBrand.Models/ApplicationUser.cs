using Microsoft.AspNetCore.Identity;

namespace ClothingBrand.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Manager? Manager { get; set; }

        public virtual ICollection<ApplicationUserProduct> ApplicationUserFavoriteProducts 
        { get; set; }
            = new HashSet<ApplicationUserProduct>();

        public virtual ICollection<ApplicationUserShoppingCart> UserShoppingCartProducts 
        { get; set; }
            = new HashSet<ApplicationUserShoppingCart>();
    }
}
