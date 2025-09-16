using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filmify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addFilmScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FilmScore",
                table: "Films",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilmScore",
                table: "Films");
        }
    }
}
