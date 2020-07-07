using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class add_auditableentity_on_companyserviceuserrole_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                schema: "Company",
                table: "CompanyServiceUserRole");
        }
    }
}
