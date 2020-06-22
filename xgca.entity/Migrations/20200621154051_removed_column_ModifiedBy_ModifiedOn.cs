using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class removed_column_ModifiedBy_ModifiedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyUser_UserId",
                schema: "Company",
                table: "CompanyUser");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "Settings",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "Settings",
                table: "AuditLog");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_UserId",
                schema: "Company",
                table: "CompanyUser",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyUser_UserId",
                schema: "Company",
                table: "CompanyUser");

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                schema: "Settings",
                table: "AuditLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "Settings",
                table: "AuditLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_UserId",
                schema: "Company",
                table: "CompanyUser",
                column: "UserId");
        }
    }
}
