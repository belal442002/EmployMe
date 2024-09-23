using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployMe.Migrations
{
    /// <inheritdoc />
    public partial class v9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Package_PackageId",
                table: "Companies");

            migrationBuilder.AlterColumn<int>(
                name: "PackageId",
                table: "Companies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Package_PackageId",
                table: "Companies",
                column: "PackageId",
                principalTable: "Package",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Package_PackageId",
                table: "Companies");

            migrationBuilder.AlterColumn<int>(
                name: "PackageId",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Package_PackageId",
                table: "Companies",
                column: "PackageId",
                principalTable: "Package",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
