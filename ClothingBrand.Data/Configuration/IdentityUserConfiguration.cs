using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingBrand.Data.Configuration
{
    public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> entity)
        {
            entity
                .HasData(this.CreateDefaultAdminUser());
        }

        private IdentityUser CreateDefaultAdminUser()
        {
            IdentityUser defaultUser = new IdentityUser
            {
                Id = "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                UserName = "admin@clothingbrand.com",
                NormalizedUserName = "ADMIN@CLOTHINGBRAND.COM",
                Email = "admin@recipesharing.com",
                NormalizedEmail = "ADMIN@CLOTHINGBRAND.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                    new IdentityUser { UserName = "admin@clothingbrand.com" },
                    "Admin123!")
            };

            return defaultUser;
        }
    }
}