using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    public partial class Update_TLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "TicketLogs");

            migrationBuilder.DropColumn(
                name: "ImageContentType",
                schema: "dbo",
                table: "TicketLogs");

            migrationBuilder.DropColumn(
                name: "ImageData",
                schema: "dbo",
                table: "TicketLogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "TicketLogs",
                type: "nvarchar(850)",
                unicode: false,
                maxLength: 850,
                nullable: false,
                defaultValue: "");

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
    }
}
