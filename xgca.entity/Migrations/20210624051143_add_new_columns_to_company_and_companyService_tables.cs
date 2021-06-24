using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class add_new_columns_to_company_and_companyService_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                schema: "Company",
                table: "CompanyService",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusName",
                schema: "Company",
                table: "Company",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 24, 5, 11, 42, 669, DateTimeKind.Utc).AddTicks(4098), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 24, 5, 11, 42, 669, DateTimeKind.Utc).AddTicks(4771) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 24, 5, 11, 42, 670, DateTimeKind.Utc).AddTicks(3862), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 24, 5, 11, 42, 670, DateTimeKind.Utc).AddTicks(3878) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceName",
                schema: "Company",
                table: "CompanyService");

            migrationBuilder.DropColumn(
                name: "StatusName",
                schema: "Company",
                table: "Company");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 16, 18, 36, 47, 303, DateTimeKind.Utc).AddTicks(7022), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 16, 18, 36, 47, 304, DateTimeKind.Utc).AddTicks(400) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 16, 18, 36, 47, 314, DateTimeKind.Utc).AddTicks(2846), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 16, 18, 36, 47, 314, DateTimeKind.Utc).AddTicks(2965) });
        }
    }
}
