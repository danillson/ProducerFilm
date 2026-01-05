using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProducerFilm.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFilmsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Films");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Director = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Year = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Films",
                columns: new[] { "Id", "CreatedAt", "Director", "Genre", "Title", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 4, 16, 7, 25, 522, DateTimeKind.Utc).AddTicks(5368), "Francis Ford Coppola", "Drama", "O Poderoso Chefão", null, 1972 },
                    { 2, new DateTime(2026, 1, 4, 16, 7, 25, 522, DateTimeKind.Utc).AddTicks(5370), "Quentin Tarantino", "Crime", "Pulp Fiction", null, 1994 },
                    { 3, new DateTime(2026, 1, 4, 16, 7, 25, 522, DateTimeKind.Utc).AddTicks(5371), "Christopher Nolan", "Ficção Científica", "A Origem", null, 2010 }
                });
        }
    }
}
