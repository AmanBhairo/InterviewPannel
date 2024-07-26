using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewPanelAvailabilitySystemAPI.Migrations
{
    public partial class initialCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InterviewRound",
                columns: table => new
                {
                    InterviewRoundId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterviewRoundName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewRound", x => x.InterviewRoundId);
                });

            migrationBuilder.CreateTable(
                name: "JobRole",
                columns: table => new
                {
                    JobRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobRoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRole", x => x.JobRoleId);
                });

            migrationBuilder.CreateTable(
                name: "Timeslot",
                columns: table => new
                {
                    TimeslotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeslotName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timeslot", x => x.TimeslotId);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JobRoleId = table.Column<int>(type: "int", nullable: true),
                    InterviewRoundId = table.Column<int>(type: "int", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsRecruiter = table.Column<bool>(type: "bit", nullable: false),
                    ChangePassword = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_InterviewRound_InterviewRoundId",
                        column: x => x.InterviewRoundId,
                        principalTable: "InterviewRound",
                        principalColumn: "InterviewRoundId");
                    table.ForeignKey(
                        name: "FK_Employee_JobRole_JobRoleId",
                        column: x => x.JobRoleId,
                        principalTable: "JobRole",
                        principalColumn: "JobRoleId");
                });

            migrationBuilder.CreateTable(
                name: "InterviewSlot",
                columns: table => new
                {
                    SlotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EmployeesEmployeeId = table.Column<int>(type: "int", nullable: false),
                    SlotDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeslotId = table.Column<int>(type: "int", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewSlot", x => x.SlotId);
                    table.ForeignKey(
                        name: "FK_InterviewSlot_Employee_EmployeesEmployeeId",
                        column: x => x.EmployeesEmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewSlot_Timeslot_TimeslotId",
                        column: x => x.TimeslotId,
                        principalTable: "Timeslot",
                        principalColumn: "TimeslotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_InterviewRoundId",
                table: "Employee",
                column: "InterviewRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_JobRoleId",
                table: "Employee",
                column: "JobRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSlot_EmployeesEmployeeId",
                table: "InterviewSlot",
                column: "EmployeesEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSlot_TimeslotId",
                table: "InterviewSlot",
                column: "TimeslotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterviewSlot");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Timeslot");

            migrationBuilder.DropTable(
                name: "InterviewRound");

            migrationBuilder.DropTable(
                name: "JobRole");
        }
    }
}
