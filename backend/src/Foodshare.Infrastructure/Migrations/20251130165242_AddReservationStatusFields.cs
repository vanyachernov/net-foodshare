using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodshare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationStatusFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "confirmed_at",
                table: "reservations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "reservations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "pickup_time",
                table: "reservations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "reservations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmed_at",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "pickup_time",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "status",
                table: "reservations");
        }
    }
}
