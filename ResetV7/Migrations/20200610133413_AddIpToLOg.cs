using Microsoft.EntityFrameworkCore.Migrations;

namespace ResetV7.Migrations
{
    public partial class AddIpToLOg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "ResetLog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ip",
                table: "ResetLog");
        }
    }
}
