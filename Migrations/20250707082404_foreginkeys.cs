using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class foreginkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                schema: "dbo",
                table: "DailyTasks",
                type: "INT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UID",
                schema: "dbo",
                table: "DailyTasks",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyTasks_LocationId",
                schema: "dbo",
                table: "DailyTasks",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTasks_UID",
                schema: "dbo",
                table: "DailyTasks",
                column: "UID");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTask_CreatedUser",
                schema: "dbo",
                table: "DailyTasks",
                column: "UID",
                principalSchema: "dbo",
                principalTable: "AppUsers",
                principalColumn: "UID");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTask_Location",
                schema: "dbo",
                table: "DailyTasks",
                column: "LocationId",
                principalSchema: "dbo",
                principalTable: "Locations",
                principalColumn: "LID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyTask_CreatedUser",
                schema: "dbo",
                table: "DailyTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyTask_Location",
                schema: "dbo",
                table: "DailyTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyTasks_LocationId",
                schema: "dbo",
                table: "DailyTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyTasks_UID",
                schema: "dbo",
                table: "DailyTasks");

            migrationBuilder.DropColumn(
                name: "LocationId",
                schema: "dbo",
                table: "DailyTasks");

            migrationBuilder.DropColumn(
                name: "UID",
                schema: "dbo",
                table: "DailyTasks");
        }
    }
}
