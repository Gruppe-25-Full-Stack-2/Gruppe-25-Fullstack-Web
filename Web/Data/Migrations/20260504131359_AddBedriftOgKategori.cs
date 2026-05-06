using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSD2491Gruppe25.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBedriftOgKategori : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategorier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KategoriNavn = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bedrifter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Organisasjonsnummer = table.Column<string>(type: "TEXT", maxLength: 9, nullable: false),
                    Navn = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Organisasjonsform = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ErAktiv = table.Column<bool>(type: "INTEGER", nullable: false),
                    Registreringsdato = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notat = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    KategoriId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bedrifter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bedrifter_Kategorier_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategorier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bedrifter_KategoriId",
                table: "Bedrifter",
                column: "KategoriId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bedrifter");

            migrationBuilder.DropTable(
                name: "Kategorier");
        }
    }
}
