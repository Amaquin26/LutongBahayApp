using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LutongBahayApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrdersMinorChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderAdress",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OrderAmountDue",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderAdress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderAmountDue",
                table: "Orders");
        }
    }
}
