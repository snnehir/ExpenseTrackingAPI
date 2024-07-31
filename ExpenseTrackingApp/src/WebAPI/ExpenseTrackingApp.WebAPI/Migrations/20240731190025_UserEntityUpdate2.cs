using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTrackingApp.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserEntityUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalExpense",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseCount",
                table: "Users");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalExpense",
                table: "Users",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
