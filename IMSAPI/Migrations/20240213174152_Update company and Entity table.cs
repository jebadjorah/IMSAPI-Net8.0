using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMSAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatecompanyandEntitytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Tbl_Entity",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Tbl_Entity",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Tbl_Entity",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Tbl_Entity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Tbl_Company",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Tbl_Company",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Tbl_Company",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Tbl_Company",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tbl_Entity");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Tbl_Entity");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Tbl_Entity");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Tbl_Entity");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tbl_Company");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Tbl_Company");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Tbl_Company");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Tbl_Company");
        }
    }
}
