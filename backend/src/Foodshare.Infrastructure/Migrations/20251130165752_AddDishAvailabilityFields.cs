using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodshare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDishAvailabilityFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_available",
                table: "dishes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "quantity_available",
                table: "dishes",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_available",
                table: "dishes");

            migrationBuilder.DropColumn(
                name: "quantity_available",
                table: "dishes");
        }
    }
}
