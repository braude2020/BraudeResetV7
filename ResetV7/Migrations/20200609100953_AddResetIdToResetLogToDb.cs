using Microsoft.EntityFrameworkCore.Migrations;

namespace ResetV7.Migrations
{
    public partial class AddResetIdToResetLogToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ResetLog",
                table: "ResetLog");

            migrationBuilder.DropColumn(
                name: "sessionId",
                table: "ResetLog");

            migrationBuilder.AddColumn<int>(
                name: "ResetID",
                table: "ResetLog",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResetLog",
                table: "ResetLog",
                column: "ResetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ResetLog",
                table: "ResetLog");

            migrationBuilder.DropColumn(
                name: "ResetID",
                table: "ResetLog");

            migrationBuilder.AddColumn<int>(
                name: "sessionId",
                table: "ResetLog",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResetLog",
                table: "ResetLog",
                column: "sessionId");
        }
    }
}
