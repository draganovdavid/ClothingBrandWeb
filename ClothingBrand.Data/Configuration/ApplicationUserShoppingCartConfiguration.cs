using ClothingBrand.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingBrand.Data.Configuration
{
    public class ApplicationUserShoppingCartConfiguration : IEntityTypeConfiguration<ApplicationUserShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserShoppingCart> builder)
        {
            builder
                .HasKey(e => new { e.ApplicationUserId, e.ProductId });

            builder
                .Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            builder
                .HasQueryFilter(e => !e.IsDeleted && !e.Product.IsDeleted);

            builder
                .HasOne(e => e.ApplicationUser)
                .WithMany()
                .HasForeignKey(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Product)
                .WithMany(p => p.UserShoppingCartItems)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}