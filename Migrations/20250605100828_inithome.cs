using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexSupport.Migrations
{
    /// <inheritdoc />
    public partial class inithome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "dbo",
                columns: table => new
                {
                    CID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(name: "Category Name", type: "NVARCHAR(50)", unicode: false, maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CATEGORYID_PK", x => x.CID);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "dbo",
                columns: table => new
                {
                    DID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(name: "Department Name", type: "NVARCHAR(50)", unicode: false, maxLength: 50, nullable: false),
                    IsActive = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DEPARTMENTID_PK", x => x.DID);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "dbo",
                columns: table => new
                {
                    LID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationName = table.Column<string>(name: "Location Name", type: "NVARCHAR(50)", unicode: false, maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("LOCATIONID_PK", x => x.LID);
                });

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

            migrationBuilder.CreateTable(
                name: "AppUsers",
                schema: "dbo",
                columns: table => new
                {
                    UID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fingerprint = table.Column<int>(type: "INT", nullable: false),
                    LoginName = table.Column<string>(type: "NVARCHAR(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(150)", unicode: false, maxLength: 150, nullable: false),
                    Fname = table.Column<string>(type: "NVARCHAR(50)", unicode: false, maxLength: 50, nullable: false),
                    Lname = table.Column<string>(type: "NVARCHAR(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR(50)", unicode: false, maxLength: 50, nullable: false),
                    JobTitle = table.Column<string>(type: "NVARCHAR(100)", unicode: false, maxLength: 100, nullable: true),
                    MobilePhone = table.Column<string>(type: "NVARCHAR(11)", unicode: false, maxLength: 11, nullable: true),
                    Phone = table.Column<string>(type: "NVARCHAR(10)", unicode: false, maxLength: 10, nullable: true),
                    Role = table.Column<string>(type: "NVARCHAR(20)", unicode: false, maxLength: 20, nullable: false),
                    EmailVerified = table.Column<string>(type: "NVARCHAR(50)", unicode: false, maxLength: 50, nullable: true),
                    Create_Date = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "(sysdatetime())"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DID = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("APPUSERUID_PK", x => x.UID);
                    table.ForeignKey(
                        name: "FK_AppUser_Department",
                        column: x => x.DID,
                        principalSchema: "dbo",
                        principalTable: "Departments",
                        principalColumn: "DID");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                schema: "dbo",
                columns: table => new
                {
                    TID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "NVARCHAR(50)", unicode: false, maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Issue = table.Column<string>(type: "NVARCHAR(850)", unicode: false, maxLength: 850, nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR(50)", unicode: false, maxLength: 50, nullable: false),
                    IsSolved = table.Column<bool>(type: "bit", nullable: true),
                    OpenDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Due_Minutes = table.Column<int>(type: "INT", nullable: true),
                    CloseDate = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    Solution = table.Column<string>(type: "NVARCHAR(850)", nullable: true),
                    Comments = table.Column<string>(type: "NVARCHAR(850)", nullable: true),
                    LID = table.Column<int>(type: "INT", nullable: false),
                    UserRate = table.Column<int>(type: "int", nullable: true),
                    UID = table.Column<int>(type: "INT", nullable: false),
                    AgentID = table.Column<int>(type: "INT", nullable: true),
                    CategoryID = table.Column<int>(type: "INT", nullable: true),
                    TicketRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Assign_Date = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    UserApprove = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    ImageData = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true),
                    ImageContentType = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TICKETID_PK", x => x.TID);
                    table.ForeignKey(
                        name: "FK_Ticket_AgentId",
                        column: x => x.AgentID,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "UID");
                    table.ForeignKey(
                        name: "FK_Ticket_Category",
                        column: x => x.CategoryID,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "CID");
                    table.ForeignKey(
                        name: "FK_Ticket_Location",
                        column: x => x.LID,
                        principalSchema: "dbo",
                        principalTable: "Locations",
                        principalColumn: "LID");
                    table.ForeignKey(
                        name: "FK_Ticket_UserId",
                        column: x => x.UID,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "UID");
                });

            migrationBuilder.CreateTable(
                name: "TicketLogs",
                schema: "dbo",
                columns: table => new
                {
                    TLOGID = table.Column<int>(name: "TLOG ID", type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TID = table.Column<int>(type: "INT", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(850)", unicode: false, maxLength: 850, nullable: false),
                    ActionTime = table.Column<DateTime>(name: "Action Time", type: "datetime2(0)", nullable: false),
                    UID = table.Column<int>(type: "INT", nullable: false),
                    ImageData = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true),
                    ImageContentType = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TLOGID_PK", x => x.TLOGID);
                    table.ForeignKey(
                        name: "FK_Tlog_AppUserId",
                        column: x => x.UID,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "UID");
                    table.ForeignKey(
                        name: "FK_Tlog_TicketId",
                        column: x => x.TID,
                        principalSchema: "dbo",
                        principalTable: "Tickets",
                        principalColumn: "TID");
                });

            migrationBuilder.CreateIndex(
                name: "APPUSEREMAIL_UQ",
                schema: "dbo",
                table: "AppUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "APPUSERFINGERPRINT_UQ",
                schema: "dbo",
                table: "AppUsers",
                column: "Fingerprint",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "APPUSERLOGINNAME_UQ",
                schema: "dbo",
                table: "AppUsers",
                column: "LoginName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "APPUSERPHONE_UQ",
                schema: "dbo",
                table: "AppUsers",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_DID",
                schema: "dbo",
                table: "AppUsers",
                column: "DID");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTasks_CategoryID",
                schema: "dbo",
                table: "DailyTasks",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketLogs_TID",
                schema: "dbo",
                table: "TicketLogs",
                column: "TID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketLogs_UID",
                schema: "dbo",
                table: "TicketLogs",
                column: "UID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AgentID",
                schema: "dbo",
                table: "Tickets",
                column: "AgentID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CategoryID",
                schema: "dbo",
                table: "Tickets",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_LID",
                schema: "dbo",
                table: "Tickets",
                column: "LID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UID",
                schema: "dbo",
                table: "Tickets",
                column: "UID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyTasks",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TicketLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Tickets",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUsers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Locations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "dbo");
        }
    }
}
