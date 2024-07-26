using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace InterviewPanelAvailabilitySystemAPI.Migrations
{
    public partial class seedRecruiter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            byte[] passwordSalt1, passwordSalt2, passwordSalt3, passwordSalt4, passwordSalt5;
            byte[] passwordHash1, passwordHash2, passwordHash3, passwordHash4, passwordHash5;

            using (var hmac1 = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt1 = hmac1.Key;
                passwordHash1 = hmac1.ComputeHash(Encoding.UTF8.GetBytes("Devanshi@123"));
            }

            using (var hmac2 = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt2 = hmac2.Key;
                passwordHash2 = hmac2.ComputeHash(Encoding.UTF8.GetBytes("Dhruva@123"));
            }

            using (var hmac3 = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt3 = hmac3.Key;
                passwordHash3 = hmac3.ComputeHash(Encoding.UTF8.GetBytes("Aman@123"));
            }

            using (var hmac4 = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt4 = hmac4.Key;
                passwordHash4 = hmac4.ComputeHash(Encoding.UTF8.GetBytes("Gaurang@123"));
            }

            using (var hmac5 = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt5 = hmac5.Key;
                passwordHash5 = hmac5.ComputeHash(Encoding.UTF8.GetBytes("Jinal@123"));
            }

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "FirstName", "LastName", "Email", "JobRoleId", "InterviewRoundId", "PasswordHash", "PasswordSalt", "IsActive", "IsAdmin", "IsRecruiter", "ChangePassword" },
                values: new object[] { "Devanshi", "Bhatevara", "dev@gmail.com", null, null, passwordHash1, passwordSalt1, true, false, true, true });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "FirstName", "LastName", "Email", "JobRoleId", "InterviewRoundId", "PasswordHash", "PasswordSalt", "IsActive", "IsAdmin", "IsRecruiter", "ChangePassword" },
                values: new object[] { "Dhruva", "Patel", "dhruva@gmail.com", null, null, passwordHash2, passwordSalt2, true, false, true, true });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "FirstName", "LastName", "Email", "JobRoleId", "InterviewRoundId", "PasswordHash", "PasswordSalt", "IsActive", "IsAdmin", "IsRecruiter", "ChangePassword" },
                values: new object[] { "Aman", "Bhairo", "amanbhairo@gmail.com", null, null, passwordHash3, passwordSalt3, true, false, true, true });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "FirstName", "LastName", "Email", "JobRoleId", "InterviewRoundId", "PasswordHash", "PasswordSalt", "IsActive", "IsAdmin", "IsRecruiter", "ChangePassword" },
                values: new object[] { "Gaurang", "Joshi", "gaurangjoshi@gmail.com", null, null, passwordHash4, passwordSalt4, true, false, true, true });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "FirstName", "LastName", "Email", "JobRoleId", "InterviewRoundId", "PasswordHash", "PasswordSalt", "IsActive", "IsAdmin", "IsRecruiter", "ChangePassword" },
                values: new object[] { "Jinal", "Solanki", "jinalsolanki@gmail.com", null, null, passwordHash5, passwordSalt5, true, false, true, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValues: new object[] { 1, 2, 3, 4, 5 }); 
        }
    }
}
