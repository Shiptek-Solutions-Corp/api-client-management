using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class add_default_values_to_companyGroupResource_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "IsDeleted",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValue: new Guid("07d196ea-adea-4cf8-8790-0cb70542bbf8"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<byte>(
                name: "IsDeleted",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(byte),
                oldDefaultValue: (byte)0);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("07d196ea-adea-4cf8-8790-0cb70542bbf8"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 1);
        }
    }
}
