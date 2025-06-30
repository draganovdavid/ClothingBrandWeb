using ClothingBrand.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingBrand.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            // Primary key
            entity
                .HasKey(p => p.Id);

            // Required fields
            entity
                .Property(p => p.Name)
                .IsRequired();

            entity
                .Property(p => p.Description)
                .IsRequired();

            entity
                .Property(p => p.Size)
                .IsRequired();

            entity
                .Property(p => p.InStock)
                .IsRequired();

            // Decimal precision
            entity
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // Optional image
            entity
                .Property(p => p.ImageUrl)
                .HasMaxLength(2048);

            // Soft delete
            entity
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(p => !p.IsDeleted);

            // Relationships
            entity
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(p => p.Gender)
                .WithMany(g => g.Products)
                .HasForeignKey(p => p.GenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasData(this.SeedProducts());
        }

        private List<Product> SeedProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = Guid.Parse("b7a1c162-1cfe-4d87-9916-2d9e373fda17"),
                    Name = "Nike Dri-FIT Running Shorts",
                    Description = "Леки и дишащи къси гащи, подходящи за спорт и ежедневие.",
                    Price = 39.99m,
                    Size = "M",
                    ImageUrl = "https://example.com/images/nike-running-shorts.jpg",
                    InStock = true,
                    AuthorId= "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                    CategoryId = 1, // T-Shirts or Shorts – както си настроил
                    GenderId = 1,   // Men
                    IsDeleted = false
                },
                new Product
                {
                    Id = Guid.Parse("7a93cc29-89b8-4fb5-8714-9be26f71f611"),
                    Name = "Nike Sportswear Club Shorts",
                    Description = "Памучни къси гащи за всекидневен комфорт с лого на Nike.",
                    Price = 34.90m,
                    Size = "L",
                    ImageUrl = "https://example.com/images/nike-club-shorts.jpg",
                    InStock = true,
                    AuthorId= "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                    CategoryId = 1,
                    GenderId = 2, // Women
                    IsDeleted = false
                },
                new Product
                {
                    Id = Guid.Parse("fdd9c18d-1a2d-452b-b504-e0d5f5c20f14"),
                    Name = "Nike Dri-FIT Legend T-Shirt",
                    Description = "Спортна тениска с къс ръкав, изработена от влагоотвеждаща материя.",
                    Price = 29.99m,
                    Size = "S",
                    ImageUrl = "https://example.com/images/nike-drifit-shirt.jpg",
                    InStock = true,
                    AuthorId= "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false
                },
                new Product
                {
                    Id = Guid.Parse("2f1c64b0-9c9d-44d2-8124-3e8bd6f3d319"),
                    Name = "Nike Air Max Graphic Tee",
                    Description = "Мека тениска с класически дизайн и принт на Air Max.",
                    Price = 24.90m,
                    Size = "XL",
                    ImageUrl = "https://example.com/images/nike-airmax-tee.jpg",
                    InStock = true,
                    AuthorId= "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                    CategoryId = 1,
                    GenderId = 3, // Kids
                    IsDeleted = false
                },
                new Product
                {
                    Id = Guid.Parse("a3d610c8-7e7c-4d7b-b2c4-150ad277d310"),
                    Name = "Basic White T-Shirt",
                    Description = "Comfortable 100% cotton T-shirt.",
                    Price = 19.99m,
                    Size = "M",
                    ImageUrl = "https://example.com/images/white-tshirt.jpg",
                    InStock = true,
                    AuthorId= "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false
                },
                new Product
                {
                    Id = Guid.Parse("d3a7a590-0873-4760-97c9-bbf7b2750116"),
                    Name = "Basic Black T-Shirt",
                    Description = "Comfortable 100% cotton T-shirt.",
                    Price = 19.99m,
                    Size = "L",
                    ImageUrl = "https://example.com/images/jeans.jpg",
                    InStock = true,
                    AuthorId= "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                    CategoryId = 3,
                    GenderId = 2,
                    IsDeleted = false
                }
            };
        }
    }
}