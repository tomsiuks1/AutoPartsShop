using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2a3374a4-ec51-441c-88cd-a999939643e5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d32eb7da-c68e-4f81-a450-1420cd99563d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e345b91f-0df8-4215-adb5-75369f39a0c4"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("59c44b49-371b-40cf-9ad2-b462d7274254"), null, "User", "User" },
                    { new Guid("98a44292-1fea-4b4b-b7ff-8d745c702c86"), null, "Member", "MEMBER" },
                    { new Guid("a5127d67-6d51-46d6-841e-1d6f5acaf4c3"), null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("59c44b49-371b-40cf-9ad2-b462d7274254"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("98a44292-1fea-4b4b-b7ff-8d745c702c86"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a5127d67-6d51-46d6-841e-1d6f5acaf4c3"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2a3374a4-ec51-441c-88cd-a999939643e5"), null, "User", "User" },
                    { new Guid("d32eb7da-c68e-4f81-a450-1420cd99563d"), null, "Member", "MEMBER" },
                    { new Guid("e345b91f-0df8-4215-adb5-75369f39a0c4"), null, "Admin", "ADMIN" }
                });
        }
    }
}
