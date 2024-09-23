using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployMe.Migrations
{
    /// <inheritdoc />
    public partial class v8 : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AvailableJob",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qualifications = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableJob", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailableJob_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Max_Job_Interviews = table.Column<int>(type: "int", nullable: false),
                    Max_Cv_Recommendation = table.Column<int>(type: "int", nullable: false),
                    Max_Vacancies = table.Column<int>(type: "int", nullable: false),
                    Active_YN = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    ApplyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Accepted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => new { x.JobId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_Applications_AvailableJob_JobId",
                        column: x => x.JobId,
                        principalTable: "AvailableJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applications_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_Companies_PackageId",
                table: "Companies",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ProfileId",
                table: "Companies",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_EmployeeId",
                table: "Applications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableJob_CompanyId",
                table: "AvailableJob",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Package_PackageId",
                table: "Companies",
                column: "PackageId",
                principalTable: "Package",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Package_PackageId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "AvailableJob");

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
                name: "IX_Companies_PackageId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ProfileId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PackageId",
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
    }
}
