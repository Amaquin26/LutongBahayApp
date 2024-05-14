using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LutongBahayApp.Migrations
{
    /// <inheritdoc />
    public partial class AspNetUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Markets_AspNetUsers_UserId",
                table: "Markets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Markets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Markets_AspNetUsers_UserId",
                table: "Markets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Markets_AspNetUsers_UserId",
                table: "Markets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Markets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Markets_AspNetUsers_UserId",
                table: "Markets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
