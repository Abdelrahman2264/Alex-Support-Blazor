using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class ISSOLVED : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Locations_locationLID",
                schema: "dbo",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_locationLID",
                schema: "dbo",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Result",
                schema: "dbo",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "locationLID",
                schema: "dbo",
                table: "Tickets");

            migrationBuilder.AddColumn<bool>(
                name: "IsSolved",
                schema: "dbo",
                table: "Tickets",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSolved",
                schema: "dbo",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "Result",
                schema: "dbo",
                table: "Tickets",
                type: "NVARCHAR(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "locationLID",
                schema: "dbo",
                table: "Tickets",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_locationLID",
                schema: "dbo",
                table: "Tickets",
                column: "locationLID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Locations_locationLID",
                schema: "dbo",
                table: "Tickets",
                column: "locationLID",
                principalSchema: "dbo",
                principalTable: "Locations",
                principalColumn: "LID");
        }
    }
}
