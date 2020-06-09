using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ResetV7.Migrations
{
    public partial class AddResetLogToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResetLog",
                columns: table => new
                {
                    sessionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    logTime = table.Column<DateTime>(nullable: false),
                    username = table.Column<string>(nullable: false),
                    mobile = table.Column<string>(maxLength: 10, nullable: false),
                    countLogin = table.Column<int>(nullable: false),
                    countOTP = table.Column<int>(nullable: false),
                    countReset = table.Column<int>(nullable: false),
                    bizUser = table.Column<bool>(nullable: false),
                    eduUser = table.Column<bool>(nullable: false),
                    sessionToken = table.Column<string>(maxLength: 6, nullable: true),
                    LogTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetLog", x => x.sessionId);
                    table.ForeignKey(
                        name: "FK_ResetLog_LogType_LogTypeId",
                        column: x => x.LogTypeId,
                        principalTable: "LogType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResetLog_LogTypeId",
                table: "ResetLog",
                column: "LogTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResetLog");
        }
    }
}
