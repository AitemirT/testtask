using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask2.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteToCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Companies_CustomerCompanyId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Companies_ExecutorCompanyId",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Companies_CustomerCompanyId",
                table: "Projects",
                column: "CustomerCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Companies_ExecutorCompanyId",
                table: "Projects",
                column: "ExecutorCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Companies_CustomerCompanyId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Companies_ExecutorCompanyId",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Companies_CustomerCompanyId",
                table: "Projects",
                column: "CustomerCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Companies_ExecutorCompanyId",
                table: "Projects",
                column: "ExecutorCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
