using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingBrand.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd");

            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "Id", "UserId" },
                values: new object[] { new Guid("080b12b6-84ab-4a23-908c-6f1835b768f9"), "a7924356-9a80-4206-963b-e71abcfa6257" });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("5aa828b1-7b16-4cc1-92f6-fa0a89d250da"),
                columns: new[] { "ManagerId", "Name" },
                values: new object[] { new Guid("080b12b6-84ab-4a23-908c-6f1835b768f9"), "Sofia Warehouse" });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20"),
                columns: new[] { "ManagerId", "Name" },
                values: new object[] { new Guid("080b12b6-84ab-4a23-908c-6f1835b768f9"), "Veliko Turnovo Warehouse" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: new Guid("080b12b6-84ab-4a23-908c-6f1835b768f9"));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 0, "a4379890-686c-4f55-8b94-3c8097bc169b", "admin@recipesharing.com", true, false, null, "ADMIN@CLOTHINGBRAND.COM", "ADMIN@CLOTHINGBRAND.COM", "AQAAAAIAAYagAAAAEKY6gal22B2v8IyEgWYFymjHmXIN7SnWwCiOTX8ymYK0LkM+WmNMBVZG3va+D6tgYA==", null, false, "86bff894-aae0-4776-ab09-39cb96b7bb2e", false, "admin@clothingbrand.com" });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("5aa828b1-7b16-4cc1-92f6-fa0a89d250da"),
                columns: new[] { "ManagerId", "Name" },
                values: new object[] { new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"), "Sofia" });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("af5efb50-807c-4dfd-8178-71c7d6ff7f20"),
                columns: new[] { "ManagerId", "Name" },
                values: new object[] { new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"), "Veliko Turnovo" });

            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "Id", "UserId" },
                values: new object[] { new Guid("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"), "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd" });
        }
    }
}
