using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LutongBahayApp.Migrations
{
    /// <inheritdoc />
    public partial class MarketUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Markets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Markets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Markets_UserId1",
                table: "Markets",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Markets_AspNetUsers_UserId1",
                table: "Markets",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Markets_AspNetUsers_UserId1",
                table: "Markets");

            migrationBuilder.DropIndex(
                name: "IX_Markets_UserId1",
                table: "Markets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Markets");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Markets");
        }
    }
}
