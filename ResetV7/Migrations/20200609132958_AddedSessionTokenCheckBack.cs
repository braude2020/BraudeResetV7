using Microsoft.EntityFrameworkCore.Migrations;

namespace ResetV7.Migrations
{
    public partial class AddedSessionTokenCheckBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "sessionToken",
                table: "ResetLog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sessionTokenCheck",
                table: "ResetLog",
                maxLength: 6,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sessionTokenCheck",
                table: "ResetLog");

            migrationBuilder.AlterColumn<string>(
                name: "sessionToken",
                table: "ResetLog",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
