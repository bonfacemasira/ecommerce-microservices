using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatePostgreSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Brand = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "Category", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "Price", "StockQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Apple", "Electronics", new DateTime(2025, 7, 17, 13, 30, 10, 407, DateTimeKind.Utc).AddTicks(1357), "Latest iPhone with advanced features", "https://example.com/iphone15pro.jpg", true, "iPhone 15 Pro", 999.99m, 50, new DateTime(2025, 7, 17, 13, 30, 10, 407, DateTimeKind.Utc).AddTicks(1357) },
                    { 2, "Samsung", "Electronics", new DateTime(2025, 7, 17, 13, 30, 10, 407, DateTimeKind.Utc).AddTicks(1437), "Premium Android smartphone", "https://example.com/galaxys24.jpg", true, "Samsung Galaxy S24", 799.99m, 30, new DateTime(2025, 7, 17, 13, 30, 10, 407, DateTimeKind.Utc).AddTicks(1438) },
                    { 3, "Apple", "Computers", new DateTime(2025, 7, 17, 13, 30, 10, 407, DateTimeKind.Utc).AddTicks(1440), "Professional laptop for developers", "https://example.com/macbookpro16.jpg", true, "MacBook Pro 16\"", 2499.99m, 20, new DateTime(2025, 7, 17, 13, 30, 10, 407, DateTimeKind.Utc).AddTicks(1441) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Brand",
                table: "Products",
                column: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category",
                table: "Products",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsActive",
                table: "Products",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
