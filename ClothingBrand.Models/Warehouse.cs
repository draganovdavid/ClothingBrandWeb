using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Data.Models
{
    public class Warehouse
    {
        [Comment("Warehouse identifier")]
        public Guid Id { get; set; }

        [Comment("Warehouse name")]
        public string Name { get; set; } = null!;

        [Comment("Warehouse location")]
        public string Location { get; set; } = null!;

        [Comment("Shows if warehouse is deleted")]
        public bool IsDeleted { get; set; }

        [Comment("Warehouse's manager")]
        public Guid? ManagerId { get; set; }

        public virtual Manager? Manager { get; set; }

        public virtual ICollection<Product> WarehouseProducts { get; set; }
            = new HashSet<Product>();
    }
}
