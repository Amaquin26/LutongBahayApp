using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LutongBahayApp.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigrationMinorChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Sales",
                table: "SoldMarketMetrics",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Sales",
                table: "Foods",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sales",
                table: "SoldMarketMetrics");

            migrationBuilder.DropColumn(
                name: "Sales",
                table: "Foods");
        }
    }
}
