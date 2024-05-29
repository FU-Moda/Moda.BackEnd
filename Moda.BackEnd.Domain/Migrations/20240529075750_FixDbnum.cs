using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moda.BackEnd.Domain.Migrations
{
    public partial class FixDbnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStocks_Warehouses_WarehouseId",
                table: "ProductStocks");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_ProductStocks_WarehouseId",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "ProductStocks");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "ProductStocks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStocks_WarehouseId",
                table: "ProductStocks",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStocks_Warehouses_WarehouseId",
                table: "ProductStocks",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
