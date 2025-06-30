using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class daysround : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                schema: "dbo",
                table: "DailyTasks",
                type: "INT",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "dbo",
                table: "DailyTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                schema: "dbo",
                table: "DailyTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "dbo",
                table: "DailyTasks");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                schema: "dbo",
                table: "DailyTasks");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "dbo",
                table: "DailyTasks",
                type: "NVARCHAR(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT",
                oldUnicode: false,
                oldMaxLength: 100);
        }
    }
}
