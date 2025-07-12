using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClothingBrand.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 0, "a4379890-686c-4f55-8b94-3c8097bc169b", "admin@recipesharing.com", true, false, null, "ADMIN@CLOTHINGBRAND.COM", "ADMIN@CLOTHINGBRAND.COM", "AQAAAAIAAYagAAAAEKY6gal22B2v8IyEgWYFymjHmXIN7SnWwCiOTX8ymYK0LkM+WmNMBVZG3va+D6tgYA==", null, false, "86bff894-aae0-4776-ab09-39cb96b7bb2e", false, "admin@clothingbrand.com" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "T-Shirts" },
                    { 2, "Hoodies" },
                    { 3, "Jeans" },
                    { 4, "Jackets" }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Men" },
                    { 2, "Women" },
                    { 3, "Kids" }
                });

            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "Id", "UserId" },
                values: new object[] { new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"), "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd" });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "Location", "ManagerId", "Name" },
                values: new object[,]
                {
                    { new Guid("5aa828b1-7b16-4cc1-92f6-fa0a89d250da"), "Sofia", new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"), "Sofia" },
                    { new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20"), "Veliko Turnovo", new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"), "Veliko Turnovo" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "GenderId", "ImageUrl", "InStock", "Name", "Price", "Size", "WarehouseId" },
                values: new object[,]
                {
                    { new Guid("08c7d54f-3bed-47fa-9e9e-1dec63777379"), 1, "Ниски маратонки 'AIR VAPORMAX 2025 FK' в Бяло", 1, "https://cdn.aboutstatic.com/file/images/2cb972f1b098663ca57e788fbc4454e1.png?bg=F4F4F5&quality=75&trim=1&height=1280&width=960", true, "Nike AIR VAPORMAX 2025 FK", 449.99m, "M", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("0eaa7866-63ca-4d5a-ae89-2615d257f7b7"), 1, "Много скъпи очила!!!", 1, "https://us.louisvuitton.com/images/is/image/lv/1/PP_VP_L/louis-vuitton-lv-waimea-sunglasses--Z1082W_PM2_Front%20view.png?wid=1300&hei=1300", true, "LV Waimea Sunglasses", 1200.00m, "M", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("186ab6b1-a4df-45b5-9a5e-68f6625cfba0"), 1, "Унисекс чехли с лого", 1, "https://fdcdn.akamaized.net/m/780x1170/products/85054/85053442/images/res_7e715b1bccef60387dc819aca31b8c47.jpg?s=V7efhJcc4Qme", true, "EA7", 89.99m, "M", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("18a6b861-4adf-45b5-9a5e-9f4e6a3d6b9a"), 1, "Чехли за плаж/баня 'Y BASE CAMP SLIDE III' в Черно", 1, "https://cdn.aboutstatic.com/file/images/24f57483a6801e9688af452bb6a85db4.jpg?brightness=0.96&quality=75&trim=1&height=1280&width=960", true, "THE NORTH FACE", 54.90m, "M", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("2e838ab5-52ff-46ba-a861-f12cce0310ae"), 1, "Мрежести плувни шорти", 1, "https://fdcdn.akamaized.net/m/780x1170/products/93445/93444094/images/res_a36898790e947d077042c677224b2504.jpg?s=0mFGRsrsUX4x", true, "GRIMELANGE", 20.99m, "M", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("3621b665-03bb-4520-9ab6-fc9dfa8f1e5a"), 1, "'Adilette Lite' в Пепел От Рози", 1, "https://cdn.aboutstatic.com/file/images/105a3c9dd2a020437a2a92db53555977.jpg?brightness=0.96&quality=75&trim=1&height=1280&width=960", true, "ADIDAS ORIGINALS Чехли", 61.90m, "S", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("464b24c7-5f7d-40fc-9acf-4e365a6dd9d8"), 1, "Десенирани шорти с връзка", 1, "https://fdcdn.akamaized.net/m/780x1170/products/84294/84293608/images/res_91fd90296f0a5aa87b99e56a973f2d38.jpg?s=54y_UYYifK2H", true, "Pepe Jeans London", 97.99m, "S", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("649376a6-1b91-4088-9d03-43231c1e1e07"), 1, "Чанта през рамо Lady Re от еко кожа с капаче", 3, "https://fdcdn.akamaized.net/m/780x1170/products/81651/81650611/images/res_43f38159b521f5bb18962d621bbb9acf.jpg?s=Fqg_alMTNmSM", true, "Valentino", 119.99m, "M", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("86cd4677-5577-43f3-870e-c3fda2b0f5c3"), 1, "Adidas x Disney Lilo & Stitch' в Синьо, Аквамарин", 1, "https://cdn.aboutstatic.com/file/images/28ba60633e1247fd11b3687ef439a41d.jpg?brightness=0.96&quality=75&trim=1&height=1280&width=960", true, "ADIDAS SPORTSWEAR Облекло за трениране", 61.90m, "S", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("b11102f8-0087-48d1-a0ec-1d4589534f5f"), 1, "Шорти за плуване 'TORTUGA' в Сапфирено Синьо", 1, "https://cdn.aboutstatic.com/file/images/0ef96422cf975be04a5bdcd1a2a2de64.png?bg=F4F4F5&quality=75&trim=1&height=1280&width=960", true, "HUGO", 137.90m, "M", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("ba2224ae-1835-441f-b54f-d7e2856ce8a0"), 1, "Бляскав горен бански с едно рамо", 1, "https://fdcdn.akamaized.net/m/780x1170/products/95610/95609240/images/res_0c084253550c278fc32e1c088c5ef460.jpg?s=QUOcA1E0pB6y", true, "Penti", 54.59m, "M", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("ccf9d86b-2bc9-4efe-87a6-a0fefe4050e7"), 1, "Polo Ralph Lauren Раница в Розово", 1, "https://cdn.aboutstatic.com/file/images/5433cf6d338be1ab629b9d598e14f44d.jpeg?brightness=0.96&quality=75&trim=1&height=1280&width=960", true, "Polo Ralph Lauren", 147.90m, "M", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("e59205cb-b689-47f7-b442-790fad17251a"), 1, " Шорти за плуване 'DAISE' в Черно", 1, "https://cdn.aboutstatic.com/file/images/5e2fdb666ae4e2a31c81a4bffbe31a22.png?bg=F4F4F5&quality=75&trim=1&height=1280&width=960", true, "HUGO", 139.90m, "M", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("e6da177b-d0fb-427e-b8fb-dc435616a4f9"), 1, "Чехли с лого", 1, "https://fdcdn.akamaized.net/m/780x1170/products/80537/80536412/images/res_3335887410769334759b95010994a7c3.jpg?s=wz3s7X8KN8k7", true, "Versace Jeans Couture", 117.99m, "M", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("e8e1f0c5-7899-41bc-812f-2152d5863ac3"), 1, "Jordan Топ в Белозелено", 3, "https://cdn.aboutstatic.com/file/images/40b72261e9c78c31f107663d8e72f6e5.png?bg=F4F4F5&quality=75&trim=1&height=1280&width=960", true, "Jordan", 33.90m, "S", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("eb05338b-c86e-4171-b1a7-fcb601b6fe43"), 1, "ELLESSE Шорти за плуване 'Distoria' в Черно", 1, "https://cdn.aboutstatic.com/file/images/dc1384a259d059709daf241ba637493e.png?bg=F4F4F5&quality=75&trim=1&height=1280&width=960", true, "ELLESSE", 89.90m, "M", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("f9ad6395-d567-47f4-805d-47337c727a62"), 1, "Функционална тениска 'Real Madrid 24/25 Home' в Бяло", 1, "https://cdn.aboutstatic.com/file/images/c2a7188bdb6e7fbfc4132268053a7fd2.png?bg=F4F4F5&quality=75&trim=1&height=1280&width=960", true, "ADIDAS PERFORMANCE", 12.90m, "S", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") },
                    { new Guid("fe321e1b-230f-4ec9-be54-de90f54b6fff"), 1, "Мрежеста блуза", 1, "https://fdcdn.akamaized.net/m/780x1170/products/77201/77200042/images/res_529f67339ae7f93a3a17566b99791715.jpg?s=nZI5QJK8Fgof", true, "June", 19.99m, "S", new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("08c7d54f-3bed-47fa-9e9e-1dec63777379"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("0eaa7866-63ca-4d5a-ae89-2615d257f7b7"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("186ab6b1-a4df-45b5-9a5e-68f6625cfba0"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("18a6b861-4adf-45b5-9a5e-9f4e6a3d6b9a"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("2e838ab5-52ff-46ba-a861-f12cce0310ae"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("3621b665-03bb-4520-9ab6-fc9dfa8f1e5a"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("464b24c7-5f7d-40fc-9acf-4e365a6dd9d8"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("649376a6-1b91-4088-9d03-43231c1e1e07"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("86cd4677-5577-43f3-870e-c3fda2b0f5c3"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b11102f8-0087-48d1-a0ec-1d4589534f5f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("ba2224ae-1835-441f-b54f-d7e2856ce8a0"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("ccf9d86b-2bc9-4efe-87a6-a0fefe4050e7"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e59205cb-b689-47f7-b442-790fad17251a"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e6da177b-d0fb-427e-b8fb-dc435616a4f9"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e8e1f0c5-7899-41bc-812f-2152d5863ac3"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("eb05338b-c86e-4171-b1a7-fcb601b6fe43"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f9ad6395-d567-47f4-805d-47337c727a62"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("fe321e1b-230f-4ec9-be54-de90f54b6fff"));

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("5aa828b1-7b16-4cc1-92f6-fa0a89d250da"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20"));

            migrationBuilder.DeleteData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd");
        }
    }
}
