using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class userprofilephoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageContentType",
                schema: "dbo",
                table: "AppUsers",
                type: "NVARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                schema: "dbo",
                table: "AppUsers",
                type: "VARBINARY(MAX)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageContentType",
                schema: "dbo",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "ImageData",
                schema: "dbo",
                table: "AppUsers");
        }
    }
}
