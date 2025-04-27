using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class FeedDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "Users",
               columns: ["Id", "Username", "Password", "Email", "Phone", "Status", "Role"],
               values:
               [
                    "68ea9747-f280-4cee-91ff-1ef3eca6e9be",
                    "UserAdmin",
                    "$2a$11$FF864wF2LSfETo6JebyITeoZSu71J.KfjXBf6v1cb.np150PcLcBe",
                    "admin@empresa.com",
                    "76878288283",
                    "1",
                    "3"
               ]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "68ea9747-f280-4cee-91ff-1ef3eca6e9be");
        }
    }
}
