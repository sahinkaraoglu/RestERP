using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alt",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FoodId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Images_FoodId",
                table: "Images",
                column: "FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Foods_FoodId",
                table: "Images",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Foods_FoodId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_FoodId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Alt",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "FoodId",
                table: "Images");
        }
    }
}
