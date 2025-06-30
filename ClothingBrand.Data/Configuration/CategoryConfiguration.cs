using ClothingBrand.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingBrand.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity
                .HasKey(c => c.Id);

            entity
                .Property(c => c.Name)
                .IsRequired();

            entity
                .HasData(this.CategoriesSeed());
        }

        private List<Category> CategoriesSeed()
        {
            return new List<Category>
            {
                new Category { Id = 1, Name = "T-Shirts" },
                new Category { Id = 2, Name = "Hoodies" },
                new Category { Id = 3, Name = "Jeans" },
                new Category { Id = 4, Name = "Jackets" }
            };
        }

    }
}
