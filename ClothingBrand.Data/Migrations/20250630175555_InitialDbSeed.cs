using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClothingBrand.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 0, "2ff7b062-aa49-42f9-b386-7f8491261943", "admin@recipesharing.com", true, false, null, "ADMIN@RECIPESHARING.COM", "ADMIN@RECIPESHARING.COM", "AQAAAAIAAYagAAAAEL9k2iLUviHYI/qu+kkem2RaUYXlWzxzYG3HriTJtF9zC+Miqb4gkBQqvCFUR0aR+w==", null, false, "81970a81-ad26-470a-8886-443af07f0a03", false, "admin@recipesharing.com" });

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
                table: "Products",
                columns: new[] { "Id", "AuthorId", "CategoryId", "Description", "GenderId", "ImageUrl", "InStock", "Name", "Price", "Size" },
                values: new object[,]
                {
                    { new Guid("2f1c64b0-9c9d-44d2-8124-3e8bd6f3d319"), "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 1, "Мека тениска с класически дизайн и принт на Air Max.", 3, "https://example.com/images/nike-airmax-tee.jpg", true, "Nike Air Max Graphic Tee", 24.90m, "XL" },
                    { new Guid("7a93cc29-89b8-4fb5-8714-9be26f71f611"), "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 1, "Памучни къси гащи за всекидневен комфорт с лого на Nike.", 2, "https://example.com/images/nike-club-shorts.jpg", true, "Nike Sportswear Club Shorts", 34.90m, "L" },
                    { new Guid("a3d610c8-7e7c-4d7b-b2c4-150ad277d310"), "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 1, "Comfortable 100% cotton T-shirt.", 1, "https://example.com/images/white-tshirt.jpg", true, "Basic White T-Shirt", 19.99m, "M" },
                    { new Guid("b7a1c162-1cfe-4d87-9916-2d9e373fda17"), "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 1, "Леки и дишащи къси гащи, подходящи за спорт и ежедневие.", 1, "https://example.com/images/nike-running-shorts.jpg", true, "Nike Dri-FIT Running Shorts", 39.99m, "M" },
                    { new Guid("d3a7a590-0873-4760-97c9-bbf7b2750116"), "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 3, "Comfortable 100% cotton T-shirt.", 2, "https://example.com/images/jeans.jpg", true, "Basic Black T-Shirt", 19.99m, "L" },
                    { new Guid("fdd9c18d-1a2d-452b-b504-e0d5f5c20f14"), "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 1, "Спортна тениска с къс ръкав, изработена от влагоотвеждаща материя.", 1, "https://example.com/images/nike-drifit-shirt.jpg", true, "Nike Dri-FIT Legend T-Shirt", 29.99m, "S" }
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
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("2f1c64b0-9c9d-44d2-8124-3e8bd6f3d319"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("7a93cc29-89b8-4fb5-8714-9be26f71f611"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a3d610c8-7e7c-4d7b-b2c4-150ad277d310"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b7a1c162-1cfe-4d87-9916-2d9e373fda17"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d3a7a590-0873-4760-97c9-bbf7b2750116"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("fdd9c18d-1a2d-452b-b504-e0d5f5c20f14"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
