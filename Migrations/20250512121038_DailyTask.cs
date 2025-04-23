using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class DailyTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyTasks",
                schema: "dbo",
                columns: table => new
                {
                    DTID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "NVARCHAR(50)", unicode: false, maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Issue = table.Column<string>(type: "NVARCHAR(850)", unicode: false, maxLength: 850, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Due_Minutes = table.Column<int>(type: "INT", nullable: false),
                    CategoryID = table.Column<int>(type: "INT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DAILYTASKID_PK", x => x.DTID);
                    table.ForeignKey(
                        name: "FK_DailyTask_Category",
                        column: x => x.CategoryID,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "CID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyTasks_CategoryID",
                schema: "dbo",
                table: "DailyTasks",
                column: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyTasks",
                schema: "dbo");
        }
    }
}
