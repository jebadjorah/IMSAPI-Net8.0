using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMSAPI.Migrations
{
    /// <inheritdoc />
    public partial class missingdbchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Users_Tbl_Roles_RoleId",
                table: "Tbl_Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Users_Tbl_Roles_RoleId",
                table: "Tbl_Users",
                column: "RoleId",
                principalTable: "Tbl_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Users_Tbl_Roles_RoleId",
                table: "Tbl_Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Users_Tbl_Roles_RoleId",
                table: "Tbl_Users",
                column: "RoleId",
                principalTable: "Tbl_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
