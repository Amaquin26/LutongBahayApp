using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LutongBahayApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSoldProductsOnFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove the SoldProducts column (if it exists)
            migrationBuilder.DropColumn(
                name: "SoldProducts",
                table: "Foods"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // This method is for rolling back the migration (optional)
            migrationBuilder.AddColumn<int>(
                name: "SoldProducts",
                table: "Foods",
                nullable: false,
                defaultValue: 0
            );
        }
    }
}
