using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace InterviewPanelAvailabilitySystemAPI.Migrations
{
    public partial class SeedInterviewer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)

        {

            byte[] passwordSalt;

            byte[] passwordHash;

            using (var hmac = new System.Security.Cryptography.HMACSHA512())

            {

                passwordSalt = hmac.Key;

                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Aman@123"));

            }

            migrationBuilder.InsertData(

              table: "Employee",

              columns: new[] { "FirstName", "LastName", "Email", "JobRoleId", "InterviewRoundId", "PasswordHash", "PasswordSalt", "IsActive", "IsAdmin", "IsRecruiter", "ChangePassword" },

              values: new object[]

              {

              "Aman",

              "Bhairo",

              "aman@gmail.com",

              1,

              1,

              passwordHash,

              passwordSalt,

              true,

              false,

              false,

              false


              });

        }

        protected override void Down(MigrationBuilder migrationBuilder)

        {

            migrationBuilder.DeleteData(

               table: "Employee",

               keyColumn: "EmployeeId",

               keyValues: new object[]

               {

              1

               });

        }
    }
}
