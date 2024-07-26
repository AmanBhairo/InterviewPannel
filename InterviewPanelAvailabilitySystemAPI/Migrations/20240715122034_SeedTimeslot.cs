using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewPanelAvailabilitySystemAPI.Migrations
{
    public partial class SeedTimeslot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                          table: "Timeslot",
                          column: "TimeslotName",
                          values: new object[]
                          {
                    "10:00AM-11:00AM",
                     "11:00AM-12:00PM",
                      "12:00PM-1:00PM",
                       "1:00PM-2:00PM",
                        "2:00PM-3:00PM",
                         "3:00PM-4:00PM",
                          "4:00PM-5:00PM",
                           "5:00PM-6:00PM",
                            "6:00PM-7:00PM",
                          });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
              table: "Timeslot",
              keyColumn: "TimeslotName",
              keyValues: new object[]
              {
                    1, 2,3,4,5,6,7,8,9
              });
        }
    }
}
