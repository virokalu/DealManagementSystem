using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Media",
                table: "Hotels");

            migrationBuilder.AddColumn<string>(
                name: "Medias",
                table: "Hotels",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Medias",
                table: "Hotels");

            migrationBuilder.AddColumn<List<string>>(
                name: "Media",
                table: "Hotels",
                type: "text[]",
                nullable: true);
        }
    }
}
