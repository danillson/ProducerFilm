using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProducerFilm.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieListHistory : Migration
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

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 16, 7, 25, 522, DateTimeKind.Utc).AddTicks(5368));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 16, 7, 25, 522, DateTimeKind.Utc).AddTicks(5370));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 16, 7, 25, 522, DateTimeKind.Utc).AddTicks(5371));

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

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 3, 16, 25, 57, 144, DateTimeKind.Utc).AddTicks(6791));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 3, 16, 25, 57, 144, DateTimeKind.Utc).AddTicks(6793));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 3, 16, 25, 57, 144, DateTimeKind.Utc).AddTicks(6795));
        }
    }
}
