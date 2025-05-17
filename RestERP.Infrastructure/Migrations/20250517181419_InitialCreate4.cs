using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Categories");

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
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TurkishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", precision: 18, scale: 2, nullable: false),
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
                table: "Foods",
                columns: new[] { "Id", "CategoryId", "CreatedById", "CreatedDate", "Description", "IsDeleted", "Name", "Price", "TurkishName", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, null, null, null, false, "Turkish Breakfast Spread", "200₺", "Kahvaltı Tabağı", null, null },
                    { 2, 1, null, null, null, false, "Mixed Breakfast (Per Person)", "300₺", "Serpme Kahvaltı", null, null },
                    { 3, 1, null, null, null, false, "Cheese Platter", "70₺", "Peynir Tabağı", null, null },
                    { 4, 1, null, null, null, false, "Olive Platter", "47,50₺", "Zeytin Tabağı", null, null },
                    { 5, 1, null, null, null, false, "Omelette", "47,50₺", "Omlet", null, null },
                    { 6, 1, null, null, null, false, "Omelette with Gorgonzola Cheese", "55₺", "Kaşarlı Omlet", null, null },
                    { 7, 1, null, null, null, false, "Mixed Omelette", "80₺", "Karışık Omlet", null, null },
                    { 8, 1, null, null, null, false, "Scrambled Eggs with Tomatoes and Green Pepper", "62,50₺", "Menemen", null, null },
                    { 9, 1, null, null, null, false, "Scrambled Eggs with Garlic Sausages", "80₺", "Sucuklu Yumurta", null, null },
                    { 10, 1, null, null, null, false, "Melted Cheese and Cornmeal", "100₺", "Mıhlama", null, null },
                    { 11, 1, null, null, null, false, "Scrambled Eggs with Pastrami", "75₺", "Pastırmalı Yumurta", null, null },
                    { 12, 1, null, null, null, false, "Fried Garlic Sausage", "85₺", "Sahanda Sucuk", null, null },
                    { 13, 1, null, null, null, false, "French Fries", "40₺", "Patates Tava", null, null },
                    { 14, 4, null, null, null, false, "Rolled Pastry", "40₺", "Mini Kalem Böreği", null, null },
                    { 15, 1, null, null, null, false, "Honey & Clotted Cream", "60₺", "Bal & Kaymak", null, null },
                    { 16, 1, null, null, null, false, "A Serving of Butter", "47,50₺", "Tereyağı Porsiyonu", null, null },
                    { 17, 1, null, null, null, false, "Tomato and Cucumber", "47,50₺", "Söğüş Tabağı", null, null },
                    { 18, 1, null, null, null, false, "Fruit Platter", "55₺", "Meyve Tabağı", null, null },
                    { 19, 2, null, null, null, false, "Stuffed Peppers", "57,50₺", "Biber Dolma", null, null },
                    { 20, 2, null, null, null, false, "Sauced Aubergine", "57,50₺", "Soslu Patlıcan", null, null },
                    { 21, 2, null, null, null, false, "Artichoke", "57,50₺", "Enginar", null, null },
                    { 22, 2, null, null, null, false, "Green Beans", "57,50₺", "Taze Fasulye", null, null },
                    { 23, 2, null, null, null, false, "Stuffed Vine Leaves", "57,50₺", "Yaprak Sarma", null, null },
                    { 24, 2, null, null, null, false, "Assorted Olive Oil Dish Platter", "80₺", "Zeytinyağı Tabağı", null, null },
                    { 25, 3, null, null, null, false, "Fresh Fries", "40₺", "Patates Tava", null, null },
                    { 26, 3, null, null, null, false, "Pastrami Pastry", "67,50₺", "Paçanga Böreği", null, null },
                    { 27, 3, null, null, null, false, "Mushroom Gratin", "75₺", "Mantar Graten", null, null },
                    { 28, 3, null, null, null, false, "Fried Mushrooms", "55₺", "Mantar Kavurma", null, null },
                    { 29, 3, null, null, null, false, "Julienne Sole Fish", "210₺", "Julyen Dil Balığı", null, null },
                    { 30, 4, null, null, null, false, "Grilled Sea Bream 550 - 650 gr.", "550₺", "Çipura Izgara 550 - 650 gr.", null, null },
                    { 31, 4, null, null, null, false, "Grilled Sea Bream 300 - 350 gr.", "310₺", "Çipura Izgara 300 - 350 gr.", null, null },
                    { 32, 4, null, null, null, false, "Fried Whiting Fish", "260₺", "Mezgit Tava", null, null },
                    { 33, 4, null, null, null, false, "Pan Seared Salmon", "415₺", "Fesleğen Soslu Somon Izgara", null, null },
                    { 34, 4, null, null, null, false, "Sea Bass Filleted", "400₺", "Levrek Fileto", null, null },
                    { 35, 4, null, null, null, false, "Grilled Sea Bass 550 - 650 gr.", "550₺", "Levrek Izgara 550 - 650 gr.", null, null },
                    { 36, 4, null, null, null, false, "Grilled Sea Bass 300 - 350 gr.", "300₺", "Levrek Izgara 300 - 350 gr.", null, null },
                    { 37, 4, null, null, null, false, "Sea Bass on the Clay Tile", "415₺", "Kiremitte Levrek", null, null },
                    { 38, 4, null, null, null, false, "Broiled Salmon Steak", "400₺", "Somon Izgara", null, null },
                    { 39, 4, null, null, null, false, "Grilled Sole Fish", "270₺", "Dil Balığı Izgara", null, null },
                    { 40, 4, null, null, null, false, "Baked Salmon", "415₺", "Somon Kavurma", null, null },
                    { 41, 5, null, null, null, false, "Horse Mackerel", "285₺", "İstavrit", null, null },
                    { 42, 5, null, null, null, false, "Blue Fish", "295₺", "Sarıkanat", null, null },
                    { 43, 5, null, null, null, false, "Bonito", "340₺", "Palamut", null, null },
                    { 44, 5, null, null, null, false, "Fried Anchovies", "250₺", "Hamsi Tava", null, null },
                    { 45, 5, null, null, null, false, "Whiting Fish", "270₺", "Mezgit", null, null },
                    { 46, 5, null, null, null, false, "Red Mullet", "290₺", "Tekir", null, null },
                    { 47, 5, null, null, null, false, "Loufer", "375₺", "Lüfer", null, null },
                    { 48, 6, null, null, null, false, "Grilled Köfte", "200₺", "Köfte Izgara", null, null },
                    { 49, 6, null, null, null, false, "Grilled Köfte with Cheese", "210₺", "Kaşarlı Köfte Izgara", null, null },
                    { 50, 6, null, null, null, false, "Chicken Shish", "130₺", "Piliç Izgara", null, null },
                    { 51, 6, null, null, null, false, "Roasting Chicken", "145₺", "Piliç Kavurma", null, null },
                    { 52, 6, null, null, null, false, "Chicken Julienne with Eggplant Purée", "145₺", "Beğendili Julyen Piliç", null, null },
                    { 53, 6, null, null, null, false, "Veal Fillet", "390₺", "Bonfile", null, null },
                    { 54, 6, null, null, null, false, "Veal Fillet with Turkish Cheese", "400₺", "Kaşarlı Bonfile", null, null },
                    { 55, 6, null, null, null, false, "Steak Fillet Julienne with Eggplant Purée", "400₺", "Beğendili Julyen Bonfile", null, null },
                    { 56, 6, null, null, null, false, "Mixed Grill", "390₺", "Karışık Izgara", null, null },
                    { 57, 6, null, null, null, false, "Shepherd's Roasting", "325₺", "Çoban Kavurma", null, null },
                    { 58, 7, null, null, null, false, "Sütlaç", "55₺", "Sütlaç", null, null },
                    { 59, 7, null, null, null, false, "Tres Leches Cake", "55₺", "Trileçe", null, null },
                    { 60, 7, null, null, null, false, "Noah's Pudding", "55₺", "Aşure", null, null },
                    { 61, 7, null, null, null, false, "Profiterole", "55₺", "Profiterol", null, null },
                    { 62, 7, null, null, null, false, "Volcanic", "80₺", "Volkanik", null, null },
                    { 63, 7, null, null, null, false, "Fruit Dessert", "60₺", "Meyveli Tatlılar", null, null },
                    { 64, 7, null, null, null, false, "Pastries", "60₺", "Hamur İşi Tatlıları", null, null },
                    { 65, 7, null, null, null, false, "Ice Cream", "47,50₺", "Dondurma Porsiyon", null, null },
                    { 66, 7, null, null, null, false, "Mixed Fruits", "55₺", "Meyve Tabağı", null, null },
                    { 67, 7, null, null, null, false, "Tahini Bread Roll", "67,50₺", "Tahinli Sarma", null, null },
                    { 68, 8, null, null, null, false, "Canned Soft Drinks", "55₺", "Meşrubat Çeşitleri", null, null },
                    { 69, 8, null, null, null, false, "Ayran", "16₺", "Ayran", null, null },
                    { 70, 8, null, null, null, false, "Mineral Water", "16₺", "Soda", null, null },
                    { 71, 8, null, null, null, false, "Frute Mineral Water", "19₺", "Meyveli Soda", null, null },
                    { 72, 8, null, null, null, false, "Orange / Pomegranate Juice", "55₺", "Sıkma Portakal / Nar Suyu", null, null },
                    { 73, 8, null, null, null, false, "Nar Çiçeği Şerbeti", "55₺", "Nar Çiçeği Şerbeti", null, null },
                    { 74, 8, null, null, null, false, "Tea", "10₺", "Çay", null, null },
                    { 75, 8, null, null, null, false, "Instant Coffee", "40₺", "Hazır Kahve", null, null },
                    { 76, 8, null, null, null, false, "Turkish Coffee", "40₺", "Türk Kahvesi", null, null },
                    { 77, 8, null, null, null, false, "Filter Coffee", "47,50₺", "Filtre Kahve", null, null },
                    { 78, 8, null, null, null, false, "Water", "8₺", "Su", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Foods_CategoryId",
                table: "Foods",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Foods_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Foods_ProductId",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "FoodCategories");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortenedEntityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TurkishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortenedEntityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TurkishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedById", "CreatedDate", "Description", "IsDeleted", "Name", "ShortenedEntityName", "TurkishName", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, null, null, false, "Breakfast", null, "Kahvaltı", null, null },
                    { 2, null, null, null, false, "OliveOilDishes", null, "Zeytinyağlılar", null, null },
                    { 3, null, null, null, false, "HotAppetizers", null, "Ara Sıcaklar", null, null },
                    { 4, null, null, null, false, "Types of Fish", null, "Balık Çeşitleri", null, null },
                    { 5, null, null, null, false, "SeasonalFish", null, "Mevsim Balıkları", null, null },
                    { 6, null, null, null, false, "TypesofGrillDishes", null, "Izgara ve Kavurmalar", null, null },
                    { 7, null, null, null, false, "Dessert", null, "Tatlılar", null, null },
                    { 8, null, null, null, false, "Drinks", null, "İçecekler", null, null }
                });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "Id", "CategoryId", "CreatedById", "CreatedDate", "Description", "IsDeleted", "Name", "Price", "ShortenedEntityName", "TurkishName", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, null, null, null, false, "Turkish Breakfast Spread", "200₺", null, "Kahvaltı Tabağı", null, null },
                    { 2, 1, null, null, null, false, "Mixed Breakfast (Per Person)", "300₺", null, "Serpme Kahvaltı", null, null },
                    { 3, 1, null, null, null, false, "Cheese Platter", "70₺", null, "Peynir Tabağı", null, null },
                    { 4, 1, null, null, null, false, "Olive Platter", "47,50₺", null, "Zeytin Tabağı", null, null },
                    { 5, 1, null, null, null, false, "Omelette", "47,50₺", null, "Omlet", null, null },
                    { 6, 1, null, null, null, false, "Omelette with Gorgonzola Cheese", "55₺", null, "Kaşarlı Omlet", null, null },
                    { 7, 1, null, null, null, false, "Mixed Omelette", "80₺", null, "Karışık Omlet", null, null },
                    { 8, 1, null, null, null, false, "Scrambled Eggs with Tomatoes and Green Pepper", "62,50₺", null, "Menemen", null, null },
                    { 9, 1, null, null, null, false, "Scrambled Eggs with Garlic Sausages", "80₺", null, "Sucuklu Yumurta", null, null },
                    { 10, 1, null, null, null, false, "Melted Cheese and Cornmeal", "100₺", null, "Mıhlama", null, null },
                    { 11, 1, null, null, null, false, "Scrambled Eggs with Pastrami", "75₺", null, "Pastırmalı Yumurta", null, null },
                    { 12, 1, null, null, null, false, "Fried Garlic Sausage", "85₺", null, "Sahanda Sucuk", null, null },
                    { 13, 1, null, null, null, false, "French Fries", "40₺", null, "Patates Tava", null, null },
                    { 14, 4, null, null, null, false, "Rolled Pastry", "40₺", null, "Mini Kalem Böreği", null, null },
                    { 15, 1, null, null, null, false, "Honey & Clotted Cream", "60₺", null, "Bal & Kaymak", null, null },
                    { 16, 1, null, null, null, false, "A Serving of Butter", "47,50₺", null, "Tereyağı Porsiyonu", null, null },
                    { 17, 1, null, null, null, false, "Tomato and Cucumber", "47,50₺", null, "Söğüş Tabağı", null, null },
                    { 18, 1, null, null, null, false, "Fruit Platter", "55₺", null, "Meyve Tabağı", null, null },
                    { 19, 2, null, null, null, false, "Stuffed Peppers", "57,50₺", null, "Biber Dolma", null, null },
                    { 20, 2, null, null, null, false, "Sauced Aubergine", "57,50₺", null, "Soslu Patlıcan", null, null },
                    { 21, 2, null, null, null, false, "Artichoke", "57,50₺", null, "Enginar", null, null },
                    { 22, 2, null, null, null, false, "Green Beans", "57,50₺", null, "Taze Fasulye", null, null },
                    { 23, 2, null, null, null, false, "Stuffed Vine Leaves", "57,50₺", null, "Yaprak Sarma", null, null },
                    { 24, 2, null, null, null, false, "Assorted Olive Oil Dish Platter", "80₺", null, "Zeytinyağı Tabağı", null, null },
                    { 25, 3, null, null, null, false, "Fresh Fries", "40₺", null, "Patates Tava", null, null },
                    { 26, 3, null, null, null, false, "Pastrami Pastry", "67,50₺", null, "Paçanga Böreği", null, null },
                    { 27, 3, null, null, null, false, "Mushroom Gratin", "75₺", null, "Mantar Graten", null, null },
                    { 28, 3, null, null, null, false, "Fried Mushrooms", "55₺", null, "Mantar Kavurma", null, null },
                    { 29, 3, null, null, null, false, "Julienne Sole Fish", "210₺", null, "Julyen Dil Balığı", null, null },
                    { 30, 4, null, null, null, false, "Grilled Sea Bream 550 - 650 gr.", "550₺", null, "Çipura Izgara 550 - 650 gr.", null, null },
                    { 31, 4, null, null, null, false, "Grilled Sea Bream 300 - 350 gr.", "310₺", null, "Çipura Izgara 300 - 350 gr.", null, null },
                    { 32, 4, null, null, null, false, "Fried Whiting Fish", "260₺", null, "Mezgit Tava", null, null },
                    { 33, 4, null, null, null, false, "Pan Seared Salmon", "415₺", null, "Fesleğen Soslu Somon Izgara", null, null },
                    { 34, 4, null, null, null, false, "Sea Bass Filleted", "400₺", null, "Levrek Fileto", null, null },
                    { 35, 4, null, null, null, false, "Grilled Sea Bass 550 - 650 gr.", "550₺", null, "Levrek Izgara 550 - 650 gr.", null, null },
                    { 36, 4, null, null, null, false, "Grilled Sea Bass 300 - 350 gr.", "300₺", null, "Levrek Izgara 300 - 350 gr.", null, null },
                    { 37, 4, null, null, null, false, "Sea Bass on the Clay Tile", "415₺", null, "Kiremitte Levrek", null, null },
                    { 38, 4, null, null, null, false, "Broiled Salmon Steak", "400₺", null, "Somon Izgara", null, null },
                    { 39, 4, null, null, null, false, "Grilled Sole Fish", "270₺", null, "Dil Balığı Izgara", null, null },
                    { 40, 4, null, null, null, false, "Baked Salmon", "415₺", null, "Somon Kavurma", null, null },
                    { 41, 5, null, null, null, false, "Horse Mackerel", "285₺", null, "İstavrit", null, null },
                    { 42, 5, null, null, null, false, "Blue Fish", "295₺", null, "Sarıkanat", null, null },
                    { 43, 5, null, null, null, false, "Bonito", "340₺", null, "Palamut", null, null },
                    { 44, 5, null, null, null, false, "Fried Anchovies", "250₺", null, "Hamsi Tava", null, null },
                    { 45, 5, null, null, null, false, "Whiting Fish", "270₺", null, "Mezgit", null, null },
                    { 46, 5, null, null, null, false, "Red Mullet", "290₺", null, "Tekir", null, null },
                    { 47, 5, null, null, null, false, "Loufer", "375₺", null, "Lüfer", null, null },
                    { 48, 6, null, null, null, false, "Grilled Köfte", "200₺", null, "Köfte Izgara", null, null },
                    { 49, 6, null, null, null, false, "Grilled Köfte with Cheese", "210₺", null, "Kaşarlı Köfte Izgara", null, null },
                    { 50, 6, null, null, null, false, "Chicken Shish", "130₺", null, "Piliç Izgara", null, null },
                    { 51, 6, null, null, null, false, "Roasting Chicken", "145₺", null, "Piliç Kavurma", null, null },
                    { 52, 6, null, null, null, false, "Chicken Julienne with Eggplant Purée", "145₺", null, "Beğendili Julyen Piliç", null, null },
                    { 53, 6, null, null, null, false, "Veal Fillet", "390₺", null, "Bonfile", null, null },
                    { 54, 6, null, null, null, false, "Veal Fillet with Turkish Cheese", "400₺", null, "Kaşarlı Bonfile", null, null },
                    { 55, 6, null, null, null, false, "Steak Fillet Julienne with Eggplant Purée", "400₺", null, "Beğendili Julyen Bonfile", null, null },
                    { 56, 6, null, null, null, false, "Mixed Grill", "390₺", null, "Karışık Izgara", null, null },
                    { 57, 6, null, null, null, false, "Shepherd's Roasting", "325₺", null, "Çoban Kavurma", null, null },
                    { 58, 7, null, null, null, false, "Sütlaç", "55₺", null, "Sütlaç", null, null },
                    { 59, 7, null, null, null, false, "Tres Leches Cake", "55₺", null, "Trileçe", null, null },
                    { 60, 7, null, null, null, false, "Noah's Pudding", "55₺", null, "Aşure", null, null },
                    { 61, 7, null, null, null, false, "Profiterole", "55₺", null, "Profiterol", null, null },
                    { 62, 7, null, null, null, false, "Volcanic", "80₺", null, "Volkanik", null, null },
                    { 63, 7, null, null, null, false, "Fruit Dessert", "60₺", null, "Meyveli Tatlılar", null, null },
                    { 64, 7, null, null, null, false, "Pastries", "60₺", null, "Hamur İşi Tatlıları", null, null },
                    { 65, 7, null, null, null, false, "Ice Cream", "47,50₺", null, "Dondurma Porsiyon", null, null },
                    { 66, 7, null, null, null, false, "Mixed Fruits", "55₺", null, "Meyve Tabağı", null, null },
                    { 67, 7, null, null, null, false, "Tahini Bread Roll", "67,50₺", null, "Tahinli Sarma", null, null },
                    { 68, 8, null, null, null, false, "Canned Soft Drinks", "55₺", null, "Meşrubat Çeşitleri", null, null },
                    { 69, 8, null, null, null, false, "Ayran", "16₺", null, "Ayran", null, null },
                    { 70, 8, null, null, null, false, "Mineral Water", "16₺", null, "Soda", null, null },
                    { 71, 8, null, null, null, false, "Frute Mineral Water", "19₺", null, "Meyveli Soda", null, null },
                    { 72, 8, null, null, null, false, "Orange / Pomegranate Juice", "55₺", null, "Sıkma Portakal / Nar Suyu", null, null },
                    { 73, 8, null, null, null, false, "Nar Çiçeği Şerbeti", "55₺", null, "Nar Çiçeği Şerbeti", null, null },
                    { 74, 8, null, null, null, false, "Tea", "10₺", null, "Çay", null, null },
                    { 75, 8, null, null, null, false, "Instant Coffee", "40₺", null, "Hazır Kahve", null, null },
                    { 76, 8, null, null, null, false, "Turkish Coffee", "40₺", null, "Türk Kahvesi", null, null },
                    { 77, 8, null, null, null, false, "Filter Coffee", "47,50₺", null, "Filtre Kahve", null, null },
                    { 78, 8, null, null, null, false, "Water", "8₺", null, "Su", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubCategoryId",
                table: "Products",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
