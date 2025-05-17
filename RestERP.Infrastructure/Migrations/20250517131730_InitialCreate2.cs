using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShortenedEntityName",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ShortenedEntityName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedById", "CreatedDate", "Description", "IsDeleted", "Name", "ShortenedEntityName", "TurkishName", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, null, null, false, "Breakfast", null, "Kahvaltı", null, null },
                    { 2, null, null, null, false, "OliveOilDishes", null, "Zeytinyağlılar", null, null },
                    { 3, null, null, null, false, "HotAppetizers", null, "AraSıcaklar", null, null },
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

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
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.AlterColumn<string>(
                name: "ShortenedEntityName",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShortenedEntityName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
