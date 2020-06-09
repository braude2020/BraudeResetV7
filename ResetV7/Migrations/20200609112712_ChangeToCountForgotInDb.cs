using Microsoft.EntityFrameworkCore.Migrations;

namespace ResetV7.Migrations
{
    public partial class ChangeToCountForgotInDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "countReset",
                table: "ResetLog");

            migrationBuilder.AddColumn<int>(
                name: "countForgot",
                table: "ResetLog",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "countForgot",
                table: "ResetLog");

            migrationBuilder.AddColumn<int>(
                name: "countReset",
                table: "ResetLog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
