using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moda.BackEnd.Domain.Migrations
{
    public partial class AddStatusToShopPackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "ShopPackages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "ShopPackages");
        }
    }
}
