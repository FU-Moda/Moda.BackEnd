using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moda.BackEnd.Domain.Migrations
{
    public partial class AddDateToRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Ratings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Ratings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_CreateBy",
                table: "Ratings",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UpdateBy",
                table: "Ratings",
                column: "UpdateBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_CreateBy",
                table: "Ratings",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_UpdateBy",
                table: "Ratings",
                column: "UpdateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_CreateBy",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_UpdateBy",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_CreateBy",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UpdateBy",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Ratings");
        }
    }
}
