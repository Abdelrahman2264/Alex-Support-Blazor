using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class ImageSave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageContentType",
                schema: "dbo",
                table: "Tickets",
                type: "NVARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                schema: "dbo",
                table: "Tickets",
                type: "VARBINARY(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageContentType",
                schema: "dbo",
                table: "TicketLogs",
                type: "NVARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                schema: "dbo",
                table: "TicketLogs",
                type: "VARBINARY(MAX)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageContentType",
                schema: "dbo",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ImageData",
                schema: "dbo",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ImageContentType",
                schema: "dbo",
                table: "TicketLogs");

            migrationBuilder.DropColumn(
                name: "ImageData",
                schema: "dbo",
                table: "TicketLogs");
        }
    }
}
