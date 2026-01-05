using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProducerFilm.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class DDDRefactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieListHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Studios = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Producers = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    Winner = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieListHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieListHistories_Year",
                table: "MovieListHistories",
                column: "Year");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieListHistories");
        }
    }
}
