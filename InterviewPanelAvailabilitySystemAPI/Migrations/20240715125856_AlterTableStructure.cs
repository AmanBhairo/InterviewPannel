using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewPanelAvailabilitySystemAPI.Migrations
{
    public partial class AlterTableStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewSlot_Employee_EmployeesEmployeeId",
                table: "InterviewSlot");

            migrationBuilder.DropIndex(
                name: "IX_InterviewSlot_EmployeesEmployeeId",
                table: "InterviewSlot");

            migrationBuilder.DropColumn(
                name: "EmployeesEmployeeId",
                table: "InterviewSlot");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSlot_EmployeeId",
                table: "InterviewSlot",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewSlot_Employee_EmployeeId",
                table: "InterviewSlot",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewSlot_Employee_EmployeeId",
                table: "InterviewSlot");

            migrationBuilder.DropIndex(
                name: "IX_InterviewSlot_EmployeeId",
                table: "InterviewSlot");

            migrationBuilder.AddColumn<int>(
                name: "EmployeesEmployeeId",
                table: "InterviewSlot",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSlot_EmployeesEmployeeId",
                table: "InterviewSlot",
                column: "EmployeesEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewSlot_Employee_EmployeesEmployeeId",
                table: "InterviewSlot",
                column: "EmployeesEmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
