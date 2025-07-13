using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class userrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserApproval",
                schema: "dbo",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "UserFeedBack",
                schema: "dbo",
                table: "Tickets",
                type: "NVARCHAR(500)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "AppUsers",
                type: "NVARCHAR(150)",
                unicode: false,
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldUnicode: false,
                oldMaxLength: 150);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserFeedBack",
                schema: "dbo",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "UserApproval",
                schema: "dbo",
                table: "Tickets",
                type: "NVARCHAR(50)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "AppUsers",
                type: "varchar(150)",
                unicode: false,
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(150)",
                oldUnicode: false,
                oldMaxLength: 150);
        }
    }
}
