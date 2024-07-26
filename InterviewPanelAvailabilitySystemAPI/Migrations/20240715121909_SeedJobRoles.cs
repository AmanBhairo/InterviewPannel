using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewPanelAvailabilitySystemAPI.Migrations
{
    public partial class SeedJobRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                          table: "JobRole",
                          column: "JobRoleName",
                          values: new object[]
                          {
                    "Developer",
                    "Tester",
                    "Business Analyst",
                          });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
               table: "JobRole",
               keyColumn: "JobRoleName",
               keyValues: new object[]
               {
                    1, 2, 3
               });
        }
    }
}
