using ClothingBrand.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using static ClothingBrand.Data.Common.EntityConstants.Product;

namespace ClothingBrand.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            // Primary key
            entity
                .HasKey(p => p.Id);

            // Required fields with constraints
            entity
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);

            entity
                .Property(p => p.Size)
                .IsRequired()
                .HasMaxLength(SizeMaxLength);

            entity
                .Property(p => p.InStock)
                .IsRequired();

            entity
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            entity
                .Property(p => p.ImageUrl)
                .HasMaxLength(ImageUrlMaxLength)
                .IsUnicode(false);

            // Soft delete default
            entity
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            // Global query filter for soft delete
            entity
                .HasQueryFilter(p => !p.IsDeleted);

            // Relationships
            entity
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(p => p.Gender)
                .WithMany(g => g.Products)
                .HasForeignKey(p => p.GenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(p => p.Warehouse)
                .WithMany(w => w.WarehouseProducts)
                .HasForeignKey(p => p.WarehouseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Optional: explicitly configure many-to-many if needed

            entity
                .HasMany(p => p.UserShoppingCartItems)
              .WithOne(sc => sc.Product)
              .HasForeignKey(sc => sc.ProductId)
              .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasMany(p => p.UserFavorites)
                  .WithOne(fav => fav.Product)
                  .HasForeignKey(fav => fav.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity
               .HasData(this.SeedProducts());
        }

        private List<Product> SeedProducts()
        {
            return new List<Product> {
                new Product
                {
                    Id = Guid.Parse("B11102F8-0087-48D1-A0EC-1D4589534F5F"),
                    Name = "HUGO",
                    Description = "Шорти за плуване 'TORTUGA' в Сапфирено Синьо",
                    Price = 137.90m,
                    Size = "M",
                    ImageUrl = "https://cdn.aboutstatic.com/file/images/0ef96422cf975be04a5bdcd1a2a2de64.png?bg=F4F4F5&quality=75&trim=1&height=1280&width=960",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("08C7D54F-3BED-47FA-9E9E-1DEC63777379"),
                    Name = "Nike AIR VAPORMAX 2025 FK",
                    Description = "Ниски маратонки 'AIR VAPORMAX 2025 FK' в Бяло",
                    Price = 449.99m,
                    Size = "M",
                    ImageUrl = "https://cdn.aboutstatic.com/file/images/2cb972f1b098663ca57e788fbc4454e1.png?bg=F4F4F5&quality=75&trim=1&height=1280&width=960",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("E8E1F0C5-7899-41BC-812F-2152D5863AC3"),
                    Name = "Jordan",
                    Description = "Jordan Топ в Белозелено",
                    Price = 33.90m,
                    Size = "S",
                    ImageUrl = "https://cdn.aboutstatic.com/file/images/40b72261e9c78c31f107663d8e72f6e5.png?bg=F4F4F5&quality=75&trim=1&height=1280&width=960",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 3,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("0EAA7866-63CA-4D5A-AE89-2615D257F7B7"),
                    Name = "LV Waimea Sunglasses",
                    Description = "Много скъпи очила!!!",
                    Price = 1200.00m,
                    Size = "M",
                    ImageUrl = "https://us.louisvuitton.com/images/is/image/lv/1/PP_VP_L/louis-vuitton-lv-waimea-sunglasses--Z1082W_PM2_Front%20view.png?wid=1300&hei=1300",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("F9AD6395-D567-47F4-805D-47337C727A62"),
                    Name = "ADIDAS PERFORMANCE",
                    Description = "Функционална тениска 'Real Madrid 24/25 Home' в Бяло",
                    Price = 12.90m,
                    Size = "S",
                    ImageUrl = "https://cdn.aboutstatic.com/file/images/c2a7188bdb6e7fbfc4132268053a7fd2.png?bg=F4F4F5&quality=75&trim=1&height=1280&width=960",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("649376A6-1B91-4088-9D03-43231C1E1E07"),
                    Name = "Valentino",
                    Description = "Чанта през рамо Lady Re от еко кожа с капаче",
                    Price = 119.99m,
                    Size = "M",
                    ImageUrl = "https://fdcdn.akamaized.net/m/780x1170/products/81651/81650611/images/res_43f38159b521f5bb18962d621bbb9acf.jpg?s=Fqg_alMTNmSM",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 3,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("464B24C7-5F7D-40FC-9ACF-4E365A6DD9D8"),
                    Name = "Pepe Jeans London",
                    Description = "Десенирани шорти с връзка",
                    Price = 97.99m,
                    Size = "S",
                    ImageUrl = "https://fdcdn.akamaized.net/m/780x1170/products/84294/84293608/images/res_91fd90296f0a5aa87b99e56a973f2d38.jpg?s=54y_UYYifK2H",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("18A6B861-4ADF-45B5-9A5E-9F4E6A3D6B9A"),
                    Name = "THE NORTH FACE",
                    Description = "Чехли за плаж/баня 'Y BASE CAMP SLIDE III' в Черно",
                    Price = 54.90m,
                    Size = "M",
                    ImageUrl = "https://cdn.aboutstatic.com/file/images/24f57483a6801e9688af452bb6a85db4.jpg?brightness=0.96&quality=75&trim=1&height=1280&width=960",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("186AB6B1-A4DF-45B5-9A5E-68F6625CFBA0"),
                    Name = "EA7",
                    Description = "Унисекс чехли с лого",
                    Price = 89.99m,
                    Size = "M",
                    ImageUrl = "https://fdcdn.akamaized.net/m/780x1170/products/85054/85053442/images/res_7e715b1bccef60387dc819aca31b8c47.jpg?s=V7efhJcc4Qme",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("E59205CB-B689-47F7-B442-790FAD17251A"),
                    Name = "HUGO",
                    Description = " Шорти за плуване 'DAISE' в Черно",
                    Price = 139.90m,
                    Size = "M",
                    ImageUrl = "https://cdn.aboutstatic.com/file/images/5e2fdb666ae4e2a31c81a4bffbe31a22.png?bg=F4F4F5&quality=75&trim=1&height=1280&width=960",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("CCF9D86B-2BC9-4EFE-87A6-A0FEFE4050E7"),
                    Name = "Polo Ralph Lauren",
                    Description = "Polo Ralph Lauren Раница в Розово",
                    Price = 147.90m,
                    Size = "M",
                    ImageUrl = "https://cdn.aboutstatic.com/file/images/5433cf6d338be1ab629b9d598e14f44d.jpeg?brightness=0.96&quality=75&trim=1&height=1280&width=960",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("86CD4677-5577-43F3-870E-C3FDA2B0F5C3"),
                    Name = "ADIDAS SPORTSWEAR Облекло за трениране",
                    Description = "Adidas x Disney Lilo & Stitch' в Синьо, Аквамарин",
                    Price = 61.90m,
                    Size = "S",
                    ImageUrl = "https://cdn.aboutstatic.com/file/images/28ba60633e1247fd11b3687ef439a41d.jpg?brightness=0.96&quality=75&trim=1&height=1280&width=960",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("BA2224AE-1835-441F-B54F-D7E2856CE8A0"),
                    Name = "Penti",
                    Description = "Бляскав горен бански с едно рамо",
                    Price = 54.59m,
                    Size = "M",
                    ImageUrl = "https://fdcdn.akamaized.net/m/780x1170/products/95610/95609240/images/res_0c084253550c278fc32e1c088c5ef460.jpg?s=QUOcA1E0pB6y",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("E6DA177B-D0FB-427E-B8FB-DC435616A4F9"),
                    Name = "Versace Jeans Couture",
                    Description = "Чехли с лого",
                    Price = 117.99m,
                    Size = "M",
                    ImageUrl = "https://fdcdn.akamaized.net/m/780x1170/products/80537/80536412/images/res_3335887410769334759b95010994a7c3.jpg?s=wz3s7X8KN8k7",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("FE321E1B-230F-4EC9-BE54-DE90F54B6FFF"),
                    Name = "June",
                    Description = "Мрежеста блуза",
                    Price = 19.99m,
                    Size = "S",
                    ImageUrl = "https://fdcdn.akamaized.net/m/780x1170/products/77201/77200042/images/res_529f67339ae7f93a3a17566b99791715.jpg?s=nZI5QJK8Fgof",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("2E838AB5-52FF-46BA-A861-F12CCE0310AE"),
                    Name = "GRIMELANGE",
                    Description = "Мрежести плувни шорти",
                    Price = 20.99m,
                    Size = "M",
                    ImageUrl = "https://fdcdn.akamaized.net/m/780x1170/products/93445/93444094/images/res_a36898790e947d077042c677224b2504.jpg?s=0mFGRsrsUX4x",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("3621B665-03BB-4520-9AB6-FC9DFA8F1E5A"),
                    Name = "ADIDAS ORIGINALS Чехли",
                    Description = "'Adilette Lite' в Пепел От Рози",
                    Price = 61.90m,
                    Size = "S",
                    ImageUrl = "https://cdn.aboutstatic.com/file/images/105a3c9dd2a020437a2a92db53555977.jpg?brightness=0.96&quality=75&trim=1&height=1280&width=960",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                },
                new Product
                {
                    Id = Guid.Parse("EB05338B-C86E-4171-B1A7-FCB601B6FE43"),
                    Name = "ELLESSE",
                    Description = "ELLESSE Шорти за плуване 'Distoria' в Черно",
                    Price = 89.90m,
                    Size = "M",
                    ImageUrl = "https://cdn.aboutstatic.com/file/images/dc1384a259d059709daf241ba637493e.png?bg=F4F4F5&quality=75&trim=1&height=1280&width=960",
                    InStock = true,
                    CategoryId = 1,
                    GenderId = 1,
                    IsDeleted = false,
                    WarehouseId = Guid.Parse("af5efb50-807c-4dfd-8178-71c7d6ff7f20")
                }
            };
        }
    }
}