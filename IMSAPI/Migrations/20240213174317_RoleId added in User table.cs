using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMSAPI.Migrations
{
    /// <inheritdoc />
    public partial class RoleIdaddedinUsertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Tbl_Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Users_RoleId",
                table: "Tbl_Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Users_Tbl_Roles_RoleId",
                table: "Tbl_Users",
                column: "RoleId",
                principalTable: "Tbl_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Users_Tbl_Roles_RoleId",
                table: "Tbl_Users");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Users_RoleId",
                table: "Tbl_Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Tbl_Users");
        }
    }
}
