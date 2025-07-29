using ClothingBrand.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingBrand.Data.Configuration
{
    public class ApplicationUserProductConfiguration : IEntityTypeConfiguration<ApplicationUserProduct>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserProduct> entity)
        {
            entity
                .HasKey(e => new { e.ApplicationUserId, e.ProductId });

            entity
                .Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(e => !e.IsDeleted && !e.Product.IsDeleted);

            entity
                .HasOne(e => e.ApplicationUser)
                .WithMany(u => u.ApplicationUserFavoriteProducts)
                .HasForeignKey(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(e => e.Product)
                .WithMany(p => p.UserFavorites)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasIndex(e => new { e.ApplicationUserId, e.ProductId });
        }
    }
}