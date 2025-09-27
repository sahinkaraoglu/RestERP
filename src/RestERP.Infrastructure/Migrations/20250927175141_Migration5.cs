using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "PrP+ZrMeO00Q+nC1ytSccRIpSvauTkdqHEBRVdRaoSE=");

            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=");

            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "sQnzu7wkTrgkQZF+0G1hi5AI3Qmzvv0bXgc5THBqi7m=");

            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "sQnzu7wkTrgkQZF+0G1hi5AI3Qmzvv0bXgc5THBqi7m=");

            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "sQnzu7wkTrgkQZF+0G1hi5AI3Qmzvv0bXgc5THBqi7m=");
        }
    }
}
