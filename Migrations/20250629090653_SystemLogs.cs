using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class SystemLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemLogs",
                schema: "dbo",
                columns: table => new
                {
                    SYSTEMLOGSID = table.Column<int>(name: "SYSTEMLOGS ID", type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ActionTime = table.Column<DateTime>(name: "Action Time", type: "datetime2(0)", nullable: false),
                    UID = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SYSTEMLOGID_PK", x => x.SYSTEMLOGSID);
                    table.ForeignKey(
                        name: "FK_SystemTLog_AppUserId",
                        column: x => x.UID,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "UID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemLogs_UID",
                schema: "dbo",
                table: "SystemLogs",
                column: "UID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemLogs",
                schema: "dbo");
        }
    }
}
