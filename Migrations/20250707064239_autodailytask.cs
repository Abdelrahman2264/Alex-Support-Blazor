using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class autodailytask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgentId",
                schema: "dbo",
                table: "DailyTasks",
                type: "INT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "dbo",
                table: "DailyTasks",
                type: "NVARCHAR(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTasks_AgentId",
                schema: "dbo",
                table: "DailyTasks",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTask_AppUser",
                schema: "dbo",
                table: "DailyTasks",
                column: "AgentId",
                principalSchema: "dbo",
                principalTable: "AppUsers",
                principalColumn: "UID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyTask_AppUser",
                schema: "dbo",
                table: "DailyTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyTasks_AgentId",
                schema: "dbo",
                table: "DailyTasks");

            migrationBuilder.DropColumn(
                name: "AgentId",
                schema: "dbo",
                table: "DailyTasks");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "dbo",
                table: "DailyTasks");
        }
    }
}
