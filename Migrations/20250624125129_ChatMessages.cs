using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class ChatMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessages",
                schema: "dbo",
                columns: table => new
                {
                    CHID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "INT", nullable: false),
                    SenderId = table.Column<int>(type: "INT", nullable: false),
                    MessageText = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 2147483647, nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: true),
                    ImageData = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true),
                    ImageContentType = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CHATMESSAGEID_PK", x => x.CHID);
                    table.ForeignKey(
                        name: "FK_APPUSER_CHATMESSAGES",
                        column: x => x.SenderId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "UID");
                    table.ForeignKey(
                        name: "FK_TICKET_CHATMESSAGES",
                        column: x => x.TicketId,
                        principalSchema: "dbo",
                        principalTable: "Tickets",
                        principalColumn: "TID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderId",
                schema: "dbo",
                table: "ChatMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_TicketId",
                schema: "dbo",
                table: "ChatMessages",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages",
                schema: "dbo");
        }
    }
}
