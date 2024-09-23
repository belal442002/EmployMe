using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployMe.Migrations
{
    /// <inheritdoc />
    public partial class v10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_AccountId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ProfileId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Companies_AccountId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ProfileId",
                table: "Companies");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AccountId",
                table: "Employees",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ProfileId",
                table: "Employees",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AccountId",
                table: "Companies",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ProfileId",
                table: "Companies",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_AccountId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ProfileId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Companies_AccountId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ProfileId",
                table: "Companies");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AccountId",
                table: "Employees",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ProfileId",
                table: "Employees",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AccountId",
                table: "Companies",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ProfileId",
                table: "Companies",
                column: "ProfileId",
                unique: true);
        }
    }
}
