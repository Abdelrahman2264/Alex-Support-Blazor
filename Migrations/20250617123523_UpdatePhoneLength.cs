using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhoneLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                schema: "dbo",
                table: "AppUsers",
                type: "NVARCHAR(20)",
                unicode: false,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(10)",
                oldUnicode: false,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MobilePhone",
                schema: "dbo",
                table: "AppUsers",
                type: "NVARCHAR(20)",
                unicode: false,
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(11)",
                oldUnicode: false,
                oldMaxLength: 11,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                schema: "dbo",
                table: "AppUsers",
                type: "NVARCHAR(10)",
                unicode: false,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(20)",
                oldUnicode: false,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MobilePhone",
                schema: "dbo",
                table: "AppUsers",
                type: "NVARCHAR(11)",
                unicode: false,
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(20)",
                oldUnicode: false,
                oldMaxLength: 11,
                oldNullable: true);
        }
    }
}
