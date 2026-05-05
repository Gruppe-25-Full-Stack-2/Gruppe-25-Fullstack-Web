using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSD2491Gruppe25.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UniktOrganisasjonsnummer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bedrifter_Organisasjonsnummer",
                table: "Bedrifter",
                column: "Organisasjonsnummer",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bedrifter_Organisasjonsnummer",
                table: "Bedrifter");
        }
    }
}
