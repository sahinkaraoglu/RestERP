using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeRole = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TurkishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TableId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TurkishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Foods_FoodCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "FoodCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Foods_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FoodCategories",
                columns: new[] { "Id", "CreatedById", "CreatedDate", "Description", "IsDeleted", "Name", "TurkishName", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, null, null, false, "Breakfast", "Kahvaltı", null, null },
                    { 2, null, null, null, false, "OliveOilDishes", "Zeytinyağlılar", null, null },
                    { 3, null, null, null, false, "HotAppetizers", "Ara Sıcaklar", null, null },
                    { 4, null, null, null, false, "Types of Fish", "Balık Çeşitleri", null, null },
                    { 5, null, null, null, false, "SeasonalFish", "Mevsim Balıkları", null, null },
                    { 6, null, null, null, false, "TypesofGrillDishes", "Izgara ve Kavurmalar", null, null },
                    { 7, null, null, null, false, "Dessert", "Tatlılar", null, null },
                    { 8, null, null, null, false, "Drinks", "İçecekler", null, null }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "Id", "CreatedById", "CreatedDate", "IsDeleted", "IsOccupied", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, null, false, null, null, null },
                    { 2, null, null, false, null, null, null },
                    { 3, null, null, false, null, null, null },
                    { 4, null, null, false, null, null, null },
                    { 5, null, null, false, null, null, null },
                    { 6, null, null, false, null, null, null },
                    { 7, null, null, false, null, null, null },
                    { 8, null, null, false, null, null, null },
                    { 9, null, null, false, null, null, null },
                    { 10, null, null, false, null, null, null },
                    { 11, null, null, false, null, null, null },
                    { 12, null, null, false, null, null, null },
                    { 13, null, null, false, null, null, null },
                    { 14, null, null, false, null, null, null },
                    { 15, null, null, false, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "Id", "CategoryId", "CreatedById", "CreatedDate", "Description", "IsDeleted", "Name", "Price", "TurkishName", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, null, null, null, false, "Turkish Breakfast Spread", 200m, "Kahvaltı Tabağı", null, null },
                    { 2, 1, null, null, null, false, "Mixed Breakfast (Per Person)", 300m, "Serpme Kahvaltı", null, null },
                    { 3, 1, null, null, null, false, "Cheese Platter", 70m, "Peynir Tabağı", null, null },
                    { 4, 1, null, null, null, false, "Olive Platter", 47.50m, "Zeytin Tabağı", null, null },
                    { 5, 1, null, null, null, false, "Omelette", 47.50m, "Omlet", null, null },
                    { 6, 1, null, null, null, false, "Omelette with Gorgonzola Cheese", 55m, "Kaşarlı Omlet", null, null },
                    { 7, 1, null, null, null, false, "Mixed Omelette", 80m, "Karışık Omlet", null, null },
                    { 8, 1, null, null, null, false, "Scrambled Eggs with Tomatoes and Green Pepper", 62.50m, "Menemen", null, null },
                    { 9, 1, null, null, null, false, "Scrambled Eggs with Garlic Sausages", 80m, "Sucuklu Yumurta", null, null },
                    { 10, 1, null, null, null, false, "Melted Cheese and Cornmeal", 100m, "Mıhlama", null, null },
                    { 11, 1, null, null, null, false, "Scrambled Eggs with Pastrami", 75m, "Pastırmalı Yumurta", null, null },
                    { 12, 1, null, null, null, false, "Fried Garlic Sausage", 85m, "Sahanda Sucuk", null, null },
                    { 13, 1, null, null, null, false, "French Fries", 40m, "Patates Tava", null, null },
                    { 14, 1, null, null, null, false, "Rolled Pastry", 40m, "Mini Kalem Böreği", null, null },
                    { 15, 1, null, null, null, false, "Honey & Clotted Cream", 60m, "Bal & Kaymak", null, null },
                    { 16, 1, null, null, null, false, "A Serving of Butter", 47.50m, "Tereyağı Porsiyonu", null, null },
                    { 17, 1, null, null, null, false, "Tomato and Cucumber", 47.50m, "Söğüş Tabağı", null, null },
                    { 18, 1, null, null, null, false, "Fruit Platter", 55m, "Meyve Tabağı", null, null },
                    { 19, 2, null, null, null, false, "Stuffed Peppers", 57.50m, "Biber Dolma", null, null },
                    { 20, 2, null, null, null, false, "Sauced Aubergine", 57.50m, "Soslu Patlıcan", null, null },
                    { 21, 2, null, null, null, false, "Artichoke", 57.50m, "Enginar", null, null },
                    { 22, 2, null, null, null, false, "Green Beans", 57.50m, "Taze Fasulye", null, null },
                    { 23, 2, null, null, null, false, "Stuffed Vine Leaves", 57.50m, "Yaprak Sarma", null, null },
                    { 24, 2, null, null, null, false, "Assorted Olive Oil Dish Platter", 80m, "Zeytinyağı Tabağı", null, null },
                    { 25, 3, null, null, null, false, "Fresh Fries", 40m, "Patates Tava", null, null },
                    { 26, 3, null, null, null, false, "Pastrami Pastry", 67.50m, "Paçanga Böreği", null, null },
                    { 27, 3, null, null, null, false, "Mushroom Gratin", 75m, "Mantar Graten", null, null },
                    { 28, 3, null, null, null, false, "Fried Mushrooms", 55m, "Mantar Kavurma", null, null },
                    { 29, 3, null, null, null, false, "Julienne Sole Fish", 210m, "Julyen Dil Balığı", null, null },
                    { 30, 4, null, null, null, false, "Grilled Sea Bream 550 - 650 gr.", 550m, "Çipura Izgara 550 - 650 gr.", null, null },
                    { 31, 4, null, null, null, false, "Grilled Sea Bream 300 - 350 gr.", 310m, "Çipura Izgara 300 - 350 gr.", null, null },
                    { 32, 4, null, null, null, false, "Fried Whiting Fish", 260m, "Mezgit Tava", null, null },
                    { 33, 4, null, null, null, false, "Pan Seared Salmon", 415m, "Fesleğen Soslu Somon Izgara", null, null },
                    { 34, 4, null, null, null, false, "Sea Bass Filleted", 400m, "Levrek Fileto", null, null },
                    { 35, 4, null, null, null, false, "Grilled Sea Bass 550 - 650 gr.", 550m, "Levrek Izgara 550 - 650 gr.", null, null },
                    { 36, 4, null, null, null, false, "Grilled Sea Bass 300 - 350 gr.", 300m, "Levrek Izgara 300 - 350 gr.", null, null },
                    { 37, 4, null, null, null, false, "Sea Bass on the Clay Tile", 415m, "Kiremitte Levrek", null, null },
                    { 38, 4, null, null, null, false, "Broiled Salmon Steak", 400m, "Somon Izgara", null, null },
                    { 39, 4, null, null, null, false, "Grilled Sole Fish", 270m, "Dil Balığı Izgara", null, null },
                    { 40, 4, null, null, null, false, "Baked Salmon", 415m, "Somon Kavurma", null, null },
                    { 41, 5, null, null, null, false, "Horse Mackerel", 285m, "İstavrit", null, null },
                    { 42, 5, null, null, null, false, "Blue Fish", 295m, "Sarıkanat", null, null },
                    { 43, 5, null, null, null, false, "Bonito", 340m, "Palamut", null, null },
                    { 44, 5, null, null, null, false, "Fried Anchovies", 250m, "Hamsi Tava", null, null },
                    { 45, 5, null, null, null, false, "Whiting Fish", 270m, "Mezgit", null, null },
                    { 46, 5, null, null, null, false, "Red Mullet", 290m, "Tekir", null, null },
                    { 47, 5, null, null, null, false, "Loufer", 375m, "Lüfer", null, null },
                    { 48, 6, null, null, null, false, "Grilled Köfte", 200m, "Köfte Izgara", null, null },
                    { 49, 6, null, null, null, false, "Grilled Köfte with Cheese", 210m, "Kaşarlı Köfte Izgara", null, null },
                    { 50, 6, null, null, null, false, "Chicken Shish", 130m, "Piliç Izgara", null, null },
                    { 51, 6, null, null, null, false, "Roasting Chicken", 145m, "Piliç Kavurma", null, null },
                    { 52, 6, null, null, null, false, "Chicken Julienne with Eggplant Purée", 145m, "Beğendili Julyen Piliç", null, null },
                    { 53, 6, null, null, null, false, "Veal Fillet", 390m, "Bonfile", null, null },
                    { 54, 6, null, null, null, false, "Veal Fillet with Turkish Cheese", 400m, "Kaşarlı Bonfile", null, null },
                    { 55, 6, null, null, null, false, "Steak Fillet Julienne with Eggplant Purée", 400m, "Beğendili Julyen Bonfile", null, null },
                    { 56, 6, null, null, null, false, "Mixed Grill", 390m, "Karışık Izgara", null, null },
                    { 57, 6, null, null, null, false, "Shepherd's Roasting", 325m, "Çoban Kavurma", null, null },
                    { 58, 7, null, null, null, false, "Sütlaç", 55m, "Sütlaç", null, null },
                    { 59, 7, null, null, null, false, "Tres Leches Cake", 55m, "Trileçe", null, null },
                    { 60, 7, null, null, null, false, "Noah's Pudding", 55m, "Aşure", null, null },
                    { 61, 7, null, null, null, false, "Profiterole", 55m, "Profiterol", null, null },
                    { 62, 7, null, null, null, false, "Volcanic", 80m, "Volkanik", null, null },
                    { 63, 7, null, null, null, false, "Fruit Dessert", 60m, "Meyveli Tatlılar", null, null },
                    { 64, 7, null, null, null, false, "Pastries", 60m, "Hamur İşi Tatlıları", null, null },
                    { 65, 7, null, null, null, false, "Ice Cream", 47.50m, "Dondurma Porsiyon", null, null },
                    { 66, 7, null, null, null, false, "Mixed Fruits", 55m, "Meyve Tabağı", null, null },
                    { 67, 7, null, null, null, false, "Tahini Bread Roll", 67.50m, "Tahinli Sarma", null, null },
                    { 68, 8, null, null, null, false, "Canned Soft Drinks", 55m, "Meşrubat Çeşitleri", null, null },
                    { 69, 8, null, null, null, false, "Ayran", 16m, "Ayran", null, null },
                    { 70, 8, null, null, null, false, "Mineral Water", 16m, "Soda", null, null },
                    { 71, 8, null, null, null, false, "Frute Mineral Water", 19m, "Meyveli Soda", null, null },
                    { 72, 8, null, null, null, false, "Orange / Pomegranate Juice", 55m, "Sıkma Portakal / Nar Suyu", null, null },
                    { 73, 8, null, null, null, false, "Nar Çiçeği Şerbeti", 55m, "Nar Çiçeği Şerbeti", null, null },
                    { 74, 8, null, null, null, false, "Tea", 10m, "Çay", null, null },
                    { 75, 8, null, null, null, false, "Instant Coffee", 40m, "Hazır Kahve", null, null },
                    { 76, 8, null, null, null, false, "Turkish Coffee", 40m, "Türk Kahvesi", null, null },
                    { 77, 8, null, null, null, false, "Filter Coffee", 47.50m, "Filtre Kahve", null, null },
                    { 78, 8, null, null, null, false, "Water", 8m, "Su", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Foods_CategoryId",
                table: "Foods",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "FoodCategories");
        }
    }
}
