using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace InterviewPanelAvailabilitySystemAPI.Migrations
{
    public partial class SeedAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)

        {

            byte[] passwordSalt;

            byte[] passwordHash;

            using (var hmac = new System.Security.Cryptography.HMACSHA512())

            {

                passwordSalt = hmac.Key;

                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Admin@123"));

            }

            migrationBuilder.InsertData(

              table: "Employee",

              columns: new[] { "FirstName", "LastName", "Email", "JobRoleId", "InterviewRoundId", "PasswordHash", "PasswordSalt", "IsActive", "IsAdmin", "IsRecruiter", "ChangePassword" },

              values: new object[]

              {

              "Admin",

              "Admin",

              "admin@gmail.com",

              null,

              null,

              passwordHash,

              passwordSalt,

              true,

              true,

              false,
              true

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
