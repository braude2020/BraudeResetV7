using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ResetV7.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResetLog",
                columns: table => new
                {
                    ResetID = table.Column<Guid>(nullable: false),
                    logTime = table.Column<DateTime>(nullable: false),
                    username = table.Column<string>(nullable: false),
                    mobile = table.Column<string>(maxLength: 10, nullable: false),
                    countReset = table.Column<int>(nullable: false),
                    countOTP = table.Column<int>(nullable: false),
                    countForgot = table.Column<int>(nullable: false),
                    bizUser = table.Column<bool>(nullable: false),
                    eduUser = table.Column<bool>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    sessionToken = table.Column<string>(nullable: true),
                    sessionTokenCheck = table.Column<string>(maxLength: 6, nullable: true),
                    LogTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetLog", x => x.ResetID);
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

            migrationBuilder.DropTable(
                name: "LogType");
        }
    }
}
