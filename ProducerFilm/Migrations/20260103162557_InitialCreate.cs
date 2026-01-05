using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProducerFilm.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Director = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Year = table.Column<int>(type: "INTEGER", nullable: true),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    { 1, new DateTime(2026, 1, 3, 16, 25, 57, 144, DateTimeKind.Utc).AddTicks(6791), "Francis Ford Coppola", "Drama", "O Poderoso Chefão", null, 1972 },
                    { 2, new DateTime(2026, 1, 3, 16, 25, 57, 144, DateTimeKind.Utc).AddTicks(6793), "Quentin Tarantino", "Crime", "Pulp Fiction", null, 1994 },
                    { 3, new DateTime(2026, 1, 3, 16, 25, 57, 144, DateTimeKind.Utc).AddTicks(6795), "Christopher Nolan", "Ficção Científica", "A Origem", null, 2010 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Films");
        }
    }
}
