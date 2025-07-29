using ClothingBrand.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static ClothingBrand.Data.Common.EntityConstants.Warehouse;

namespace ClothingBrand.Data.Configuration
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> entity)
        {
            // Primary key
            entity
                .HasKey(w => w.Id);

            // Properties
            entity
                .Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(w => w.Location)
                .IsRequired()
                .HasMaxLength(LocationMaxLength);

            entity
                .Property(w => w.IsDeleted)
                .HasDefaultValue(false);

            // Global query filter
            entity
                .HasQueryFilter(w => !w.IsDeleted);

            // Relationships
            entity
                .HasMany(w => w.WarehouseProducts)
                .WithOne(p => p.Warehouse)
                .HasForeignKey(p => p.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(w => w.Manager)
                .WithMany(m => m.ManagedWarehouses)
                .HasForeignKey(w => w.ManagerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            entity
                .HasData(new Warehouse
                {
                    Id = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20"),
                    Name = "Veliko Turnovo Warehouse",
                    Location = "Veliko Turnovo",
                    IsDeleted = false,
                    ManagerId = Guid.Parse("080b12b6-84ab-4a23-908c-6f1835b768f9")
                },
                    new Warehouse
                    {
                        Id = Guid.Parse("5aa828b1-7b16-4cc1-92f6-fa0a89d250da"),
                        Name = "Sofia Warehouse",
                        Location = "Sofia",
                        IsDeleted = false,
                        ManagerId = Guid.Parse("080b12b6-84ab-4a23-908c-6f1835b768f9")
                    }
                );
        }
    }
}