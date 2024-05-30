using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moda.BackEnd.Domain.Migrations
{
    public partial class AddRegisteredDateToShopOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredDate",
                table: "ShopPackages",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisteredDate",
                table: "ShopPackages");
        }
    }
}
