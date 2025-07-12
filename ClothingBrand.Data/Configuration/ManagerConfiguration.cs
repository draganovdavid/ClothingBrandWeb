using ClothingBrand.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingBrand.Data.Configuration
{
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> entity)
        {
            entity
                .HasKey(m => m.Id);

            entity
                .Property(m => m.UserId)
                .IsRequired();

            entity
                .Property(m => m.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(m => m.User)
                .WithOne()
                .HasForeignKey<Manager>(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasIndex(m => new { m.UserId })
                .IsUnique();

            entity
                .HasQueryFilter(m => !m.IsDeleted);

            entity
                .HasData(new Manager
                {
                    Id = Guid.Parse("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"),
                    UserId = "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                    IsDeleted = false
                });
        }
    }
}
