using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moda.BackEnd.Domain.Migrations
{
    public partial class AddRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "02962efa-1273-46c0-b103-7167b1742ef3", "02962efa-1273-46c0-b103-7167b1742ef3", "CUSTOMER", "customer" },
                    { "6a32e12a-60b5-4d93-8306-82231e1232d7", "6a32e12a-60b5-4d93-8306-82231e1232d7", "ADMIN", "admin" },
                    { "814f9270-78f5-4503-b7d3-0c567e5812ba", "814f9270-78f5-4503-b7d3-0c567e5812ba", "SHOP", "shop" },
                    { "85b6791c-49d8-4a61-ad0b-8274ec27e764", "85b6791c-49d8-4a61-ad0b-8274ec27e764", "STAFF", "staff" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "02962efa-1273-46c0-b103-7167b1742ef3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a32e12a-60b5-4d93-8306-82231e1232d7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "814f9270-78f5-4503-b7d3-0c567e5812ba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85b6791c-49d8-4a61-ad0b-8274ec27e764");
        }
    }
}
