using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moda.BackEnd.Domain.Migrations
{
    public partial class FixIncorrectField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTags_Products_TagId",
                table: "ProductTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_AccountId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_AccountId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Ratings");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTags_Tags_TagId",
                table: "ProductTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTags_Tags_TagId",
                table: "ProductTags");

            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_AccountId",
                table: "Ratings",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTags_Products_TagId",
                table: "ProductTags",
                column: "TagId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_AccountId",
                table: "Ratings",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
