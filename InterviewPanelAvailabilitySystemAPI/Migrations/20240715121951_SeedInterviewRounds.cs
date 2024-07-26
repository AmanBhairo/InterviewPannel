using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewPanelAvailabilitySystemAPI.Migrations
{
    public partial class SeedInterviewRounds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                          table: "InterviewRound",
                          column: "InterviewRoundName",
                          values: new object[]
                          {
                    "Technical",
                    "Manager"
                          });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
               table: "InterviewRound",
               keyColumn: "InterviewRoundName",
               keyValues: new object[]
               {
                    1, 2
               });
        }
    }
}
