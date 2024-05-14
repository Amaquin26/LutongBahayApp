using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LutongBahayApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Markets_AspNetUsers_UserId1",
                table: "Markets");

            migrationBuilder.DropIndex(
                name: "IX_Markets_UserId1",
                table: "Markets");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Markets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Markets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Markets_UserId",
                table: "Markets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Markets_AspNetUsers_UserId",
                table: "Markets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Markets_AspNetUsers_UserId",
                table: "Markets");

            migrationBuilder.DropIndex(
                name: "IX_Markets_UserId",
                table: "Markets");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Markets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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
    }
}
