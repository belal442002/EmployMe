using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployMe.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ProfileId",
                table: "Employees",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ProfileId",
                table: "Companies",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Profiles_ProfileId",
                table: "Companies",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Profiles_ProfileId",
                table: "Employees",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Profiles_ProfileId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Profiles_ProfileId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ProfileId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ProfileId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Companies");
        }
    }
}
