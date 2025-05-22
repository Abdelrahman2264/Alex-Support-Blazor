using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class Notification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "dbo",
                columns: table => new
                {
                    NID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromUserId = table.Column<int>(type: "INT", nullable: false),
                    ToUserId = table.Column<int>(type: "INT", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    AppUserUID = table.Column<int>(type: "INT", nullable: true),
                    AppUserUID1 = table.Column<int>(type: "INT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("NOTIFICATIONNID_PK", x => x.NID);
                    table.ForeignKey(
                        name: "FK_Notifications_AppUsers_AppUserUID",
                        column: x => x.AppUserUID,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "UID");
                    table.ForeignKey(
                        name: "FK_Notifications_AppUsers_AppUserUID1",
                        column: x => x.AppUserUID1,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "UID");
                    table.ForeignKey(
                        name: "FK_Notifications_AppUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_AppUsers_ToUserId",
                        column: x => x.ToUserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AppUserUID",
                schema: "dbo",
                table: "Notifications",
                column: "AppUserUID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AppUserUID1",
                schema: "dbo",
                table: "Notifications",
                column: "AppUserUID1");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_FromUserId",
                schema: "dbo",
                table: "Notifications",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ToUserId",
                schema: "dbo",
                table: "Notifications",
                column: "ToUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "dbo");
        }
    }
}
