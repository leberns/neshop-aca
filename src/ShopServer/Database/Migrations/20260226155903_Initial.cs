using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pgvector;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    BrandId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusType = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductEmbeddings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    GeneratedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    Deployment = table.Column<string>(type: "text", nullable: false),
                    Embedding = table.Column<Vector>(type: "vector(1536)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEmbeddings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductEmbeddings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    BundlePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    CartId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCarts_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCarts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "ApexRidge" },
                    { 2, "TerraFlow" },
                    { 3, "ZenithPath" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Tent" },
                    { 2, "Hiking Shoes" },
                    { 3, "Backpacks" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "UserName" },
                values: new object[,]
                {
                    { 1, "john.blue@example.ch", "John Blue" },
                    { 2, "marry.green@example.ch", "Marry Green" }
                });

            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "Id", "CustomerId", "StatusType" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandId", "CategoryId", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, 1, "Fictional ultralight tent for high-altitude camping.", "Aura 2 Ultralight", 315.00m },
                    { 2, 2, 1, "A 2-person heavy-duty basecamp tent built for extreme conditions.", "Citadel Fortress 2", 285.99m },
                    { 3, 2, 1, "A heavy-duty basecamp tent built for extreme conditions.", "Citadel Fortress 4", 485.99m },
                    { 4, 1, 2, "Synthetic mesh trail runners for rapid elevation gain.", "Vanguard Low-Pro", 145.00m },
                    { 5, 2, 2, "Reinforced leather boots for technical rocky scrambles.", "Titan-Claw Boots", 189.50m },
                    { 6, 2, 3, "Expedition-grade pack with modular external storage.", "Atlas Hauler 75L", 265.00m },
                    { 7, 2, 3, "Minimalist hydration pack for peak bagging.", "Swift-Pulse 15", 75.00m },
                    { 8, 3, 3, "Roll-top waterproof backpack for the modern explorer.", "Nomad Versa-Pack", 99.00m }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "Name", "ProductId" },
                values: new object[,]
                {
                    { 1, "p1-tent-aura-2-ultralight", 1 },
                    { 2, "p2-tent-citadel-fortress-2", 2 },
                    { 3, "p2-tent-citadel-fortress-2-background", 2 },
                    { 4, "p3-tent-citadel-fortress-4", 3 },
                    { 5, "p4-shoes-vanguard-low-pro", 4 },
                    { 6, "p5-shoes-titan-claw-boots", 5 },
                    { 7, "p6-backpack-atlas-hauler-75l-blue", 6 },
                    { 8, "p6-backpack-atlas-hauler-75l-braun", 6 },
                    { 9, "p6-backpack-atlas-hauler-75l-green", 6 },
                    { 10, "p7-backpack-swift-pulse-15l", 7 },
                    { 11, "p8-backpack-nomad-versa-pack", 8 }
                });

            migrationBuilder.InsertData(
                table: "ProductCarts",
                columns: new[] { "Id", "BundlePrice", "CartId", "ProductId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 315.00m, 1, 1, 1, 315.00m },
                    { 2, 150.00m, 1, 7, 3, 75.00m }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "CreatedAt", "CustomerId", "ProductId", "Rating", "Status", "Text", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2025, 2, 14, 20, 35, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), 1, 1, 5, 1, "Great product!", new DateTimeOffset(new DateTime(2025, 2, 14, 20, 35, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)) },
                    { 2, new DateTimeOffset(new DateTime(2025, 2, 15, 13, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), 2, 1, 3, 2, null, new DateTimeOffset(new DateTime(2025, 2, 16, 17, 34, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)) },
                    { 3, new DateTimeOffset(new DateTime(2025, 2, 16, 17, 35, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), 2, 1, 2, 1, "Too small for big people!", new DateTimeOffset(new DateTime(2025, 2, 16, 17, 35, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCarts_CartId",
                table: "ProductCarts",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCarts_ProductId",
                table: "ProductCarts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmbeddings_ProductId",
                table: "ProductEmbeddings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Price",
                table: "Products",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomerId",
                table: "Reviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "ProductCarts");

            migrationBuilder.DropTable(
                name: "ProductEmbeddings");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
