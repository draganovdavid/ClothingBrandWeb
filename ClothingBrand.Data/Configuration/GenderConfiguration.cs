using ClothingBrand.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static ClothingBrand.Data.Common.EntityConstants.Gender;

namespace ClothingBrand.Data.Configuration
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> entity)
        {
            entity
                .HasKey(g => g.Id);

            entity
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(GenderMaxLength);

            entity
                .HasData(this.GendersSeed());
        }

        private List<Gender> GendersSeed()
        {
            return new List<Gender>
            {
                new Gender { Id = 1, Name = "Men" },
                new Gender { Id = 2, Name = "Women" },
                new Gender { Id = 3, Name = "Kids" }
            };
        }
    }
}