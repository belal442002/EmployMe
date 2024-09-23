using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployMe.Migrations
{
    /// <inheritdoc />
    public partial class v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "CV",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "VAT_Number",
                table: "Companies",
                newName: "TaxIDUrl");

            migrationBuilder.RenameColumn(
                name: "TAX_Card",
                table: "Companies",
                newName: "CommercialRegisterUrl");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CVurl",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "CVurl",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "TaxIDUrl",
                table: "Companies",
                newName: "VAT_Number");

            migrationBuilder.RenameColumn(
                name: "CommercialRegisterUrl",
                table: "Companies",
                newName: "TAX_Card");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Profiles",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "CV",
                table: "Employees",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
