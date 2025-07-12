using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Data.Models
{
    [Comment("Manager in the system")]
    public class Manager
    {
        [Comment("Manager identifier")]
        public Guid Id { get; set; }

        [Comment("Manager's user entity")]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public virtual ICollection<Warehouse> ManagedWarehouses { get; set; }
            = new HashSet<Warehouse>();
    }
}
