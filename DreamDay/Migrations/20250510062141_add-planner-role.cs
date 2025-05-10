using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamDay.Migrations
{
    /// <inheritdoc />
    public partial class addplannerrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PlannerId",
                table: "Weddings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weddings_PlannerId",
                table: "Weddings",
                column: "PlannerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_Users_PlannerId",
                table: "Weddings",
                column: "PlannerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_Users_PlannerId",
                table: "Weddings");

            migrationBuilder.DropIndex(
                name: "IX_Weddings_PlannerId",
                table: "Weddings");

            migrationBuilder.DropColumn(
                name: "PlannerId",
                table: "Weddings");
        }
    }
}
