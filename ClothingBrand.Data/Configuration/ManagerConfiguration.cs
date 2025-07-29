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
                .WithOne(u => u.Manager)
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
                    Id = Guid.Parse("080b12b6-84ab-4a23-908c-6f1835b768f9"),
                    UserId = "a7924356-9a80-4206-963b-e71abcfa6257",
                    IsDeleted = false
                });
        }
    }
}
