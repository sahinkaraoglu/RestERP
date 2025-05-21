using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "Alt", "CreatedById", "CreatedDate", "FoodId", "IsDeleted", "Path", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Kahvaltı Tabağı", null, null, 1, false, "/img/Food/Breakfast/KahvaltiTabagi.jpg", null, null },
                    { 2, "Serpme Kahvaltı", null, null, 2, false, "/img/Food/Breakfast/SerpmeKahvalti.jpg", null, null },
                    { 3, "Peynir Tabağı", null, null, 3, false, "/img/Food/Breakfast/PeynirTabagi.jpeg", null, null },
                    { 4, "Zeytin Tabağı", null, null, 4, false, "/img/Food/Breakfast/ZeytinTabagi.jpeg", null, null },
                    { 5, "Omlet", null, null, 5, false, "/img/Food/Breakfast/Omlet.jpg", null, null },
                    { 6, "Kaşarlı Omlet", null, null, 6, false, "/img/Food/Breakfast/KasarliOmlet.jpg", null, null },
                    { 7, "Karışık Omlet", null, null, 7, false, "/img/Food/Breakfast/KarisikOmlet.jpg", null, null },
                    { 8, "Menemen", null, null, 8, false, "/img/Food/Breakfast/Menemen.jpg", null, null },
                    { 9, "Sucuklu Yumurta", null, null, 9, false, "/img/Food/Breakfast/SucukluYumurta.jpeg", null, null },
                    { 10, "Mıhlama", null, null, 10, false, "/img/Food/Breakfast/Mihlama.jpg", null, null },
                    { 11, "Pastırmalı Yumurta", null, null, 11, false, "/img/Food/Breakfast/PastirmaliYumurta.jpg", null, null },
                    { 12, "Sahanda Sucuk", null, null, 12, false, "/img/Food/Breakfast/SahandaSucuk.jpg", null, null },
                    { 13, "Patates Tava", null, null, 13, false, "/img/Food/Breakfast/PatatesTava.jpg", null, null },
                    { 14, "Mini Kalem Boreği", null, null, 14, false, "/img/Food/Breakfast/MiniKalemBoregi.jpg", null, null },
                    { 15, "Bal Kaymak", null, null, 15, false, "/img/Food/Breakfast/BalKaymak.jpg", null, null },
                    { 16, "Tereyağı Porsiyonu", null, null, 16, false, "/img/Food/Breakfast/TereyagiPorsiyonu.jpg", null, null },
                    { 17, "Söğüş Tabağı", null, null, 17, false, "/img/Food/Breakfast/SogusTabagi.jpg", null, null },
                    { 18, "Meyve Tabağı", null, null, 18, false, "/img/Food/Breakfast/MeyveTabagi.jpg", null, null },
                    { 19, "Biber Dolması", null, null, 19, false, "/img/Food/OliveOilDishes/biberdolmasi.jpg", null, null },
                    { 20, "Soslu Patlıcan", null, null, 20, false, "/img/Food/OliveOilDishes/soslupatlican.jpg", null, null },
                    { 21, "Enginar", null, null, 21, false, "/img/Food/OliveOilDishes/enginar.jpg", null, null },
                    { 22, "Taze Fasulye", null, null, 22, false, "/img/Food/OliveOilDishes/tazefasulye.jpg", null, null },
                    { 23, "Yaprak Sarma", null, null, 23, false, "/img/Food/OliveOilDishes/yapraksarma.jpg", null, null },
                    { 24, "Zeytinyağı Tabağı", null, null, 24, false, "/img/Food/OliveOilDishes/zeytinyagitabagi.jpg", null, null },
                    { 25, "Patates Tava", null, null, 25, false, "/img/Food/HotAppetizers/patatestava.jpg", null, null },
                    { 26, "Paçanga Böreği", null, null, 26, false, "/img/Food/HotAppetizers/pacangaboregi.jpg", null, null },
                    { 27, "Mantar Graten", null, null, 27, false, "/img/Food/HotAppetizers/mantargraten.jpg", null, null },
                    { 28, "Mantar Kavurma", null, null, 28, false, "/img/Food/HotAppetizers/mantarkavurma.jpg", null, null },
                    { 29, "Julyen Dil Balığı", null, null, 29, false, "/img/Food/HotAppetizers/julyendilbaligi.jpg", null, null },
                    { 30, "Çipura Izgara", null, null, 30, false, "/img/Food/TypesofFish/cipuraizgara.jpg", null, null },
                    { 31, "Çipura Izgara Büyük Boy", null, null, 31, false, "/img/Food/TypesofFish/cipuraizgarabuyuk.jpg", null, null },
                    { 32, "Mezgit Tava", null, null, 32, false, "/img/Food/TypesofFish/mezgittava.jpg", null, null },
                    { 33, "Fesleğen Soslu Somon", null, null, 33, false, "/img/Food/TypesofFish/feslegenlisomonizgara.jpg", null, null },
                    { 34, "Levrek Fileto", null, null, 34, false, "/img/Food/TypesofFish/levrekfileto.jpg", null, null },
                    { 35, "Levrek Izgara", null, null, 35, false, "/img/Food/TypesofFish/levrekizgara.jpg", null, null },
                    { 36, "Levrek Izgara Büyük Boy", null, null, 36, false, "/img/Food/TypesofFish/levrekizgarabuyuk.jpg", null, null },
                    { 37, "Kiremitte Levrek", null, null, 37, false, "/img/Food/TypesofFish/kiremittelevrek.jpg", null, null },
                    { 38, "Somon Izgara", null, null, 38, false, "/img/Food/TypesofFish/somonizgara.jpg", null, null },
                    { 39, "Dil Balığı Izgara", null, null, 39, false, "/img/Food/TypesofFish/dilbaligiizgara.jpg", null, null },
                    { 40, "Somon Kavurma", null, null, 40, false, "/img/Food/TypesofFish/somonkavurma.jpg", null, null },
                    { 41, "İstavrit", null, null, 41, false, "/img/Food/SeasonalFish/istavrit.jpg", null, null },
                    { 42, "Sarıkanat", null, null, 42, false, "/img/Food/SeasonalFish/sarikanat.jpg", null, null },
                    { 43, "Palamut", null, null, 43, false, "/img/Food/SeasonalFish/palamut.jpg", null, null },
                    { 44, "Hamsi Tava", null, null, 44, false, "/img/Food/SeasonalFish/hamsitava.jpg", null, null },
                    { 45, "Mezgit", null, null, 45, false, "/img/Food/SeasonalFish/mezgit.jpg", null, null },
                    { 46, "Tekir", null, null, 46, false, "/img/Food/SeasonalFish/tekir.jpg", null, null },
                    { 47, "Lüfer", null, null, 47, false, "/img/Food/SeasonalFish/lufer.jpg", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 47);
        }
    }
}
