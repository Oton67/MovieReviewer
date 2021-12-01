using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieReviewer.Data.Migrations
{
    public partial class Roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d9e8f8cd-2b9d-4ad4-9ec2-343de2781bcf", "2c5a5976-dcd9-44f9-8311-2f0cf8f05106", "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "da4397e1-dddc-43f6-b92e-44719499aa3a", "facd8d6a-f629-406c-9c01-c663efdcddd4", "Manager", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9e8f8cd-2b9d-4ad4-9ec2-343de2781bcf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da4397e1-dddc-43f6-b92e-44719499aa3a");
        }
    }
}
