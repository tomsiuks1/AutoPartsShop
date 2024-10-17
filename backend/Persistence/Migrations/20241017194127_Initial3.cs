using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogItems_Categories_CategoryId",
                table: "CatalogItems");

            migrationBuilder.DropIndex(
                name: "IX_CatalogItems_CategoryId",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "CatalogItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogItems_Categories_Id",
                table: "CatalogItems",
                column: "Id",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogItems_Categories_Id",
                table: "CatalogItems");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "CatalogItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CategoryId",
                table: "CatalogItems",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogItems_Categories_CategoryId",
                table: "CatalogItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
