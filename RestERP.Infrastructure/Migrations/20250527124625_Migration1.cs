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
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleType = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

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
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false),
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
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
                    { 1, null, null, false, false, null, null },
                    { 2, null, null, false, false, null, null },
                    { 3, null, null, false, false, null, null },
                    { 4, null, null, false, false, null, null },
                    { 5, null, null, false, false, null, null },
                    { 6, null, null, false, false, null, null },
                    { 7, null, null, false, false, null, null },
                    { 8, null, null, false, false, null, null },
                    { 9, null, null, false, false, null, null },
                    { 10, null, null, false, false, null, null },
                    { 11, null, null, false, false, null, null },
                    { 12, null, null, false, false, null, null },
                    { 13, null, null, false, false, null, null },
                    { 14, null, null, false, false, null, null },
                    { 15, null, null, false, false, null, null }
                });

            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "Id", "CategoryId", "CreatedById", "CreatedDate", "Description", "IsDeleted", "Name", "Price", "TurkishName", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, null, null, "Çeşitli peynirler, zeytinler, domatesler, salatalıklar ve ekmeklerle servis edilen geleneksel bir Türk kahvaltısı.", false, "Turkish Breakfast Spread", 200m, "Kahvaltı Tabağı", null, null },
                    { 2, 1, null, null, "Kişi başı serpme usulü sunulan zengin kahvaltı tabağı. Çeşitli peynirler, reçeller, bal, kaymak ve diğer kahvaltılıklar içerir.", false, "Mixed Breakfast (Per Person)", 300m, "Serpme Kahvaltı", null, null },
                    { 3, 1, null, null, "Çeşitli yerel ve ithal peynirlerden oluşan seçki. Taze ekmek ile servis edilir.", false, "Cheese Platter", 70m, "Peynir Tabağı", null, null },
                    { 4, 1, null, null, "Farklı yörelerden toplanmış yeşil ve siyah zeytinler.", false, "Olive Platter", 47.50m, "Zeytin Tabağı", null, null },
                    { 5, 1, null, null, "Taze yumurtalardan hazırlanan sade omlet.", false, "Omelette", 47.50m, "Omlet", null, null },
                    { 6, 1, null, null, "Kaşar peyniri ile hazırlanmış lezzetli omlet.", false, "Omelette with Gorgonzola Cheese", 55m, "Kaşarlı Omlet", null, null },
                    { 7, 1, null, null, "Peynir, domates, biber ve diğer malzemelerle hazırlanmış karışık omlet.", false, "Mixed Omelette", 80m, "Karışık Omlet", null, null },
                    { 8, 1, null, null, "Domates, yeşil biber ve yumurta ile hazırlanan geleneksel Türk kahvaltı yemeği.", false, "Scrambled Eggs with Tomatoes and Green Pepper", 62.50m, "Menemen", null, null },
                    { 9, 1, null, null, "Kızarmış sucuk parçaları ile pişirilmiş yumurta.", false, "Scrambled Eggs with Garlic Sausages", 80m, "Sucuklu Yumurta", null, null },
                    { 10, 1, null, null, "Karadeniz bölgesine özgü, mısır unu ve eritilmiş peynir ile hazırlanan geleneksel yemek.", false, "Melted Cheese and Cornmeal", 100m, "Mıhlama", null, null },
                    { 11, 1, null, null, "İnce dilimlenmiş pastırma ile pişirilmiş yumurta.", false, "Scrambled Eggs with Pastrami", 75m, "Pastırmalı Yumurta", null, null },
                    { 12, 1, null, null, "Sahanda kızartılmış dilimlenmiş sucuk.", false, "Fried Garlic Sausage", 85m, "Sahanda Sucuk", null, null },
                    { 13, 1, null, null, "Kızarmış taze patates.", false, "French Fries", 40m, "Patates Tava", null, null },
                    { 14, 1, null, null, "İçi peynirli mini kalem börekleri.", false, "Rolled Pastry", 40m, "Mini Kalem Böreği", null, null },
                    { 15, 1, null, null, "Taze kaymak ve organik bal ile servis edilir.", false, "Honey & Clotted Cream", 60m, "Bal & Kaymak", null, null },
                    { 16, 1, null, null, "Taze tereyağı porsiyonu.", false, "A Serving of Butter", 47.50m, "Tereyağı Porsiyonu", null, null },
                    { 17, 1, null, null, "Doğranmış domates, salatalık ve yeşilliklerden oluşan söğüş tabağı.", false, "Tomato and Cucumber", 47.50m, "Söğüş Tabağı", null, null },
                    { 18, 1, null, null, "Mevsim meyvelerinden oluşan tabak.", false, "Fruit Platter", 55m, "Meyve Tabağı", null, null },
                    { 19, 2, null, null, "Pirinç ve baharatlarla doldurulmuş taze biberler.", false, "Stuffed Peppers", 57.50m, "Biber Dolma", null, null },
                    { 20, 2, null, null, "Özel sos ile hazırlanmış patlıcan yemeği.", false, "Sauced Aubergine", 57.50m, "Soslu Patlıcan", null, null },
                    { 21, 2, null, null, "Zeytinyağlı enginar yemeği.", false, "Artichoke", 57.50m, "Enginar", null, null },
                    { 22, 2, null, null, "Zeytinyağlı taze fasulye yemeği.", false, "Green Beans", 57.50m, "Taze Fasulye", null, null },
                    { 23, 2, null, null, "Pirinç ve baharatlarla doldurulmuş asma yaprağı sarması.", false, "Stuffed Vine Leaves", 57.50m, "Yaprak Sarma", null, null },
                    { 24, 2, null, null, "Çeşitli zeytinyağlı yemeklerden oluşan tabak.", false, "Assorted Olive Oil Dish Platter", 80m, "Zeytinyağı Tabağı", null, null },
                    { 25, 3, null, null, "Taze patateslerden hazırlanmış kızartma.", false, "Fresh Fries", 40m, "Patates Tava", null, null },
                    { 26, 3, null, null, "İçi pastırma ile doldurulmuş çıtır börek.", false, "Pastrami Pastry", 67.50m, "Paçanga Böreği", null, null },
                    { 27, 3, null, null, "Fırında kaşar peyniri ile gratine edilmiş mantar.", false, "Mushroom Gratin", 75m, "Mantar Graten", null, null },
                    { 28, 3, null, null, "Tereyağında kavrulmuş taze mantarlar.", false, "Fried Mushrooms", 55m, "Mantar Kavurma", null, null },
                    { 29, 3, null, null, "İnce doğranmış dil balığından hazırlanan özel sote.", false, "Julienne Sole Fish", 210m, "Julyen Dil Balığı", null, null },
                    { 30, 4, null, null, "550-650 gr ağırlığında ızgara çipura balığı.", false, "Grilled Sea Bream 550 - 650 gr.", 550m, "Çipura Izgara 550 - 650 gr.", null, null },
                    { 31, 4, null, null, "300-350 gr ağırlığında ızgara çipura balığı.", false, "Grilled Sea Bream 300 - 350 gr.", 310m, "Çipura Izgara 300 - 350 gr.", null, null },
                    { 32, 4, null, null, "Un ile kaplanıp kızartılmış mezgit balığı.", false, "Fried Whiting Fish", 260m, "Mezgit Tava", null, null },
                    { 33, 4, null, null, "Fesleğen sosu ile servis edilen ızgara somon balığı.", false, "Pan Seared Salmon", 415m, "Fesleğen Soslu Somon Izgara", null, null },
                    { 34, 4, null, null, "Filetolanmış levrek balığı.", false, "Sea Bass Filleted", 400m, "Levrek Fileto", null, null },
                    { 35, 4, null, null, "550-650 gr ağırlığında ızgara levrek balığı.", false, "Grilled Sea Bass 550 - 650 gr.", 550m, "Levrek Izgara 550 - 650 gr.", null, null },
                    { 36, 4, null, null, "300-350 gr ağırlığında ızgara levrek balığı.", false, "Grilled Sea Bass 300 - 350 gr.", 300m, "Levrek Izgara 300 - 350 gr.", null, null },
                    { 37, 4, null, null, "Kiremit üzerinde özel soslar ile pişirilmiş levrek balığı.", false, "Sea Bass on the Clay Tile", 415m, "Kiremitte Levrek", null, null },
                    { 38, 4, null, null, "Taze ızgara somon balığı.", false, "Broiled Salmon Steak", 400m, "Somon Izgara", null, null },
                    { 39, 4, null, null, "Izgara edilmiş dil balığı.", false, "Grilled Sole Fish", 270m, "Dil Balığı Izgara", null, null },
                    { 40, 4, null, null, "Özel soslar ile kavrulmuş somon balığı.", false, "Baked Salmon", 415m, "Somon Kavurma", null, null },
                    { 41, 5, null, null, "Mevsiminde taze istavrit balığı.", false, "Horse Mackerel", 285m, "İstavrit", null, null },
                    { 42, 5, null, null, "Taze sarıkanat balığı.", false, "Blue Fish", 295m, "Sarıkanat", null, null },
                    { 43, 5, null, null, "Mevsiminde taze palamut balığı.", false, "Bonito", 340m, "Palamut", null, null },
                    { 44, 5, null, null, "Mısır unu ile kaplanıp kızartılmış hamsi balığı.", false, "Fried Anchovies", 250m, "Hamsi Tava", null, null },
                    { 45, 5, null, null, "Taze mezgit balığı.", false, "Whiting Fish", 270m, "Mezgit", null, null },
                    { 46, 5, null, null, "Taze tekir balığı.", false, "Red Mullet", 290m, "Tekir", null, null },
                    { 47, 5, null, null, "Mevsiminde taze lüfer balığı.", false, "Loufer", 375m, "Lüfer", null, null },
                    { 48, 6, null, null, "Özel baharatlarla hazırlanmış ızgara köfte.", false, "Grilled Köfte", 200m, "Köfte Izgara", null, null },
                    { 49, 6, null, null, "Kaşar peyniri ile servis edilen ızgara köfte.", false, "Grilled Köfte with Cheese", 210m, "Kaşarlı Köfte Izgara", null, null },
                    { 50, 6, null, null, "Özel marine edilmiş tavuk şiş.", false, "Chicken Shish", 130m, "Piliç Izgara", null, null },
                    { 51, 6, null, null, "Özel baharatlarla kavurulmuş tavuk parçaları.", false, "Roasting Chicken", 145m, "Piliç Kavurma", null, null },
                    { 52, 6, null, null, "Közlenmiş patlıcan ezmesi üzerinde julyen doğranmış tavuk parçaları.", false, "Chicken Julienne with Eggplant Purée", 145m, "Beğendili Julyen Piliç", null, null },
                    { 53, 6, null, null, "Tercihe göre pişirilmiş dana bonfile.", false, "Veal Fillet", 390m, "Bonfile", null, null },
                    { 54, 6, null, null, "Kaşar peyniri ile servis edilen dana bonfile.", false, "Veal Fillet with Turkish Cheese", 400m, "Kaşarlı Bonfile", null, null },
                    { 55, 6, null, null, "Közlenmiş patlıcan ezmesi üzerinde julyen doğranmış dana bonfile.", false, "Steak Fillet Julienne with Eggplant Purée", 400m, "Beğendili Julyen Bonfile", null, null },
                    { 56, 6, null, null, "Köfte, pirzola, tavuk ve diğer etlerin ızgara karışımı.", false, "Mixed Grill", 390m, "Karışık Izgara", null, null },
                    { 57, 6, null, null, "Sebzeler ile hazırlanmış özel kavurma.", false, "Shepherd's Roasting", 325m, "Çoban Kavurma", null, null },
                    { 58, 7, null, null, "Geleneksel fırında pişirilmiş sütlaç.", false, "Sütlaç", 55m, "Sütlaç", null, null },
                    { 59, 7, null, null, "Üç farklı süt ile hazırlanmış özel tatlı.", false, "Tres Leches Cake", 55m, "Trileçe", null, null },
                    { 60, 7, null, null, "Geleneksel aşure tatlısı.", false, "Noah's Pudding", 55m, "Aşure", null, null },
                    { 61, 7, null, null, "Çikolata sosu ile servis edilen profiterol.", false, "Profiterole", 55m, "Profiterol", null, null },
                    { 62, 7, null, null, "Sıcak çikolata sosu ile servis edilen özel tatlı.", false, "Volcanic", 80m, "Volkanik", null, null },
                    { 63, 7, null, null, "Mevsim meyveleriyle hazırlanmış tatlı çeşitleri.", false, "Fruit Dessert", 60m, "Meyveli Tatlılar", null, null },
                    { 64, 7, null, null, "Geleneksel hamur işi tatlılar.", false, "Pastries", 60m, "Hamur İşi Tatlıları", null, null },
                    { 65, 7, null, null, "Çeşitli aromalarda dondurma porsiyonu.", false, "Ice Cream", 47.50m, "Dondurma Porsiyon", null, null },
                    { 66, 7, null, null, "Mevsim meyvelerinden hazırlanmış meyve tabağı.", false, "Mixed Fruits", 55m, "Meyve Tabağı", null, null },
                    { 67, 7, null, null, "Tahin ile hazırlanmış geleneksel tatlı.", false, "Tahini Bread Roll", 67.50m, "Tahinli Sarma", null, null },
                    { 68, 8, null, null, "Kutu içecek çeşitleri.", false, "Canned Soft Drinks", 55m, "Meşrubat Çeşitleri", null, null },
                    { 69, 8, null, null, "Geleneksel yoğurt içeceği.", false, "Ayran", 16m, "Ayran", null, null },
                    { 70, 8, null, null, "Maden suyu.", false, "Mineral Water", 16m, "Soda", null, null },
                    { 71, 8, null, null, "Çeşitli aromalarda meyveli maden suyu.", false, "Frute Mineral Water", 19m, "Meyveli Soda", null, null },
                    { 72, 8, null, null, "Taze sıkılmış portakal veya nar suyu.", false, "Orange / Pomegranate Juice", 55m, "Sıkma Portakal / Nar Suyu", null, null },
                    { 73, 8, null, null, "Geleneksel nar çiçeği şerbeti.", false, "Nar Çiçeği Şerbeti", 55m, "Nar Çiçeği Şerbeti", null, null },
                    { 74, 8, null, null, "Demli Türk çayı.", false, "Tea", 10m, "Çay", null, null },
                    { 75, 8, null, null, "Nescafe, sade veya sütlü olarak servis edilir.", false, "Instant Coffee", 40m, "Hazır Kahve", null, null },
                    { 76, 8, null, null, "Geleneksel köpüklü Türk kahvesi.", false, "Turkish Coffee", 40m, "Türk Kahvesi", null, null },
                    { 77, 8, null, null, "Taze çekilmiş kahve çekirdeklerinden hazırlanmış filtre kahve.", false, "Filter Coffee", 47.50m, "Filtre Kahve", null, null },
                    { 78, 8, null, null, "0.5L içme suyu.", false, "Water", 8m, "Su", null, null }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "CreatedById", "CreatedDate", "FoodId", "IsDeleted", "Path", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, null, 1, false, "/img/Food/Breakfast/KahvaltiTabagi.jpg", null, null },
                    { 2, null, null, 2, false, "/img/Food/Breakfast/SerpmeKahvalti.jpg", null, null },
                    { 3, null, null, 3, false, "/img/Food/Breakfast/PeynirTabagi.jpeg", null, null },
                    { 4, null, null, 4, false, "/img/Food/Breakfast/ZeytinTabagi.jpeg", null, null },
                    { 5, null, null, 5, false, "/img/Food/Breakfast/Omlet.jpg", null, null },
                    { 6, null, null, 6, false, "/img/Food/Breakfast/KasarliOmlet.jpg", null, null },
                    { 7, null, null, 7, false, "/img/Food/Breakfast/KarisikOmlet.jpg", null, null },
                    { 8, null, null, 8, false, "/img/Food/Breakfast/Menemen.jpg", null, null },
                    { 9, null, null, 9, false, "/img/Food/Breakfast/SucukluYumurta.jpeg", null, null },
                    { 10, null, null, 10, false, "/img/Food/Breakfast/Mihlama.jpg", null, null },
                    { 11, null, null, 11, false, "/img/Food/Breakfast/PastirmaliYumurta.jpg", null, null },
                    { 12, null, null, 12, false, "/img/Food/Breakfast/SahandaSucuk.jpg", null, null },
                    { 13, null, null, 13, false, "/img/Food/Breakfast/PatatesTava.jpg", null, null },
                    { 14, null, null, 14, false, "/img/Food/Breakfast/MiniKalemBoregi.jpg", null, null },
                    { 15, null, null, 15, false, "/img/Food/Breakfast/BalKaymak.jpg", null, null },
                    { 16, null, null, 16, false, "/img/Food/Breakfast/TereyagiPorsiyonu.jpg", null, null },
                    { 17, null, null, 17, false, "/img/Food/Breakfast/SogusTabagi.jpg", null, null },
                    { 18, null, null, 18, false, "/img/Food/Breakfast/MeyveTabagi.jpg", null, null },
                    { 19, null, null, 19, false, "/img/Food/OliveOilDishes/biberdolmasi.jpg", null, null },
                    { 20, null, null, 20, false, "/img/Food/OliveOilDishes/soslupatlican.jpg", null, null },
                    { 21, null, null, 21, false, "/img/Food/OliveOilDishes/enginar.jpg", null, null },
                    { 22, null, null, 22, false, "/img/Food/OliveOilDishes/tazefasulye.jpg", null, null },
                    { 23, null, null, 23, false, "/img/Food/OliveOilDishes/yapraksarma.jpg", null, null },
                    { 24, null, null, 24, false, "/img/Food/OliveOilDishes/zeytinyagitabagi.jpg", null, null },
                    { 25, null, null, 25, false, "/img/Food/HotAppetizers/patatestava.jpg", null, null },
                    { 26, null, null, 26, false, "/img/Food/HotAppetizers/pacangaboregi.jpg", null, null },
                    { 27, null, null, 27, false, "/img/Food/HotAppetizers/mantargraten.jpg", null, null },
                    { 28, null, null, 28, false, "/img/Food/HotAppetizers/mantarkavurma.jpg", null, null },
                    { 29, null, null, 29, false, "/img/Food/HotAppetizers/julyendilbaligi.jpg", null, null },
                    { 30, null, null, 30, false, "/img/Food/TypesofFish/cipuraizgara.jpg", null, null },
                    { 31, null, null, 31, false, "/img/Food/TypesofFish/cipuraizgarabuyuk.jpg", null, null },
                    { 32, null, null, 32, false, "/img/Food/TypesofFish/mezgittava.jpg", null, null },
                    { 33, null, null, 33, false, "/img/Food/TypesofFish/feslegenlisomonizgara.jpg", null, null },
                    { 34, null, null, 34, false, "/img/Food/TypesofFish/levrekfileto.jpg", null, null },
                    { 35, null, null, 35, false, "/img/Food/TypesofFish/levrekizgara.jpg", null, null },
                    { 36, null, null, 36, false, "/img/Food/TypesofFish/levrekizgarabuyuk.jpg", null, null },
                    { 37, null, null, 37, false, "/img/Food/TypesofFish/kiremittelevrek.jpg", null, null },
                    { 38, null, null, 38, false, "/img/Food/TypesofFish/somonizgara.jpg", null, null },
                    { 39, null, null, 39, false, "/img/Food/TypesofFish/dilbaligiizgara.jpg", null, null },
                    { 40, null, null, 40, false, "/img/Food/TypesofFish/somonkavurma.jpg", null, null },
                    { 41, null, null, 41, false, "/img/Food/SeasonalFish/istavrit.jpg", null, null },
                    { 42, null, null, 42, false, "/img/Food/SeasonalFish/sarikanat.jpg", null, null },
                    { 43, null, null, 43, false, "/img/Food/SeasonalFish/palamut.jpg", null, null },
                    { 44, null, null, 44, false, "/img/Food/SeasonalFish/hamsitava.jpg", null, null },
                    { 45, null, null, 45, false, "/img/Food/SeasonalFish/mezgit.jpg", null, null },
                    { 46, null, null, 46, false, "/img/Food/SeasonalFish/tekir.jpg", null, null },
                    { 47, null, null, 47, false, "/img/Food/SeasonalFish/lufer.jpg", null, null },
                    { 48, null, null, 48, false, "/img/Food/TypesofGrillDishes/kofteizgara.jpg", null, null },
                    { 49, null, null, 49, false, "/img/Food/TypesofGrillDishes/kasarlikofteizgara.jpg", null, null },
                    { 50, null, null, 50, false, "/img/Food/TypesofGrillDishes/pilicizgara.jpg", null, null },
                    { 51, null, null, 51, false, "/img/Food/TypesofGrillDishes/pilickavurma.jpg", null, null },
                    { 52, null, null, 52, false, "/img/Food/TypesofGrillDishes/begendilijulyenpilic.jpg", null, null },
                    { 53, null, null, 53, false, "/img/Food/TypesofGrillDishes/bonfile.jpg", null, null },
                    { 54, null, null, 54, false, "/img/Food/TypesofGrillDishes/kasarlibonfile.jpg", null, null },
                    { 55, null, null, 55, false, "/img/Food/TypesofGrillDishes/begendilijulyenbonfile.jpg", null, null },
                    { 56, null, null, 56, false, "/img/Food/TypesofGrillDishes/karisikizgara.jpg", null, null },
                    { 57, null, null, 57, false, "/img/Food/TypesofGrillDishes/cobankavurma.jpg", null, null },
                    { 58, null, null, 58, false, "/img/Food/Dessert/sutlac.jpg", null, null },
                    { 59, null, null, 59, false, "/img/Food/Dessert/trilece.jpg", null, null },
                    { 60, null, null, 60, false, "/img/Food/Dessert/asure.jpg", null, null },
                    { 61, null, null, 61, false, "/img/Food/Dessert/profiterol.jpg", null, null },
                    { 62, null, null, 62, false, "/img/Food/Dessert/volkanik.jpg", null, null },
                    { 63, null, null, 63, false, "/img/Food/Dessert/meyvelitatlilar.jpg", null, null },
                    { 64, null, null, 64, false, "/img/Food/Dessert/hamurisitatlilari.jpg", null, null },
                    { 65, null, null, 65, false, "/img/Food/Dessert/dondurmaporsiyon.jpg", null, null },
                    { 66, null, null, 66, false, "/img/Food/Dessert/meyvetabagi.jpg", null, null },
                    { 67, null, null, 67, false, "/img/Food/Dessert/tahinlisarma.jpg", null, null },
                    { 68, null, null, 68, false, "/img/Food/Drinks/mesrubatcesitleri.jpg", null, null },
                    { 69, null, null, 69, false, "/img/Food/Drinks/ayran.jpg", null, null },
                    { 70, null, null, 70, false, "/img/Food/Drinks/soda.jpg", null, null },
                    { 71, null, null, 71, false, "/img/Food/Drinks/meyvelisoda.jpg", null, null },
                    { 72, null, null, 72, false, "/img/Food/Drinks/sikmaportakalnarsuyu.jpg", null, null },
                    { 73, null, null, 73, false, "/img/Food/Drinks/narcicegiserbeti.jpg", null, null },
                    { 74, null, null, 74, false, "/img/Food/Drinks/cay.jpg", null, null },
                    { 75, null, null, 75, false, "/img/Food/Drinks/hazirkahve.jpg", null, null },
                    { 76, null, null, 76, false, "/img/Food/Drinks/turkkahvesi.jpg", null, null },
                    { 77, null, null, 77, false, "/img/Food/Drinks/filtrekahve.jpg", null, null },
                    { 78, null, null, 78, false, "/img/Food/Drinks/su.jpg", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_CategoryId",
                table: "Foods",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_FoodId",
                table: "Images",
                column: "FoodId");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "FoodCategories");
        }
    }
}
