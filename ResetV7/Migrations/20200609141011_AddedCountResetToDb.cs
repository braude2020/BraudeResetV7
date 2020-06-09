using Microsoft.EntityFrameworkCore.Migrations;

namespace ResetV7.Migrations
{
    public partial class AddedCountResetToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "countLogin",
                table: "ResetLog");

            migrationBuilder.AddColumn<int>(
                name: "countReset",
                table: "ResetLog",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "countReset",
                table: "ResetLog");

            migrationBuilder.AddColumn<int>(
                name: "countLogin",
                table: "ResetLog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
