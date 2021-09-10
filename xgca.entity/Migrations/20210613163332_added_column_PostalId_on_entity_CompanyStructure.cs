using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_column_PostalId_on_entity_CompanyStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostalId",
                schema: "Company",
                table: "CompanyStructure",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 13, 16, 33, 31, 370, DateTimeKind.Utc).AddTicks(1980), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 13, 16, 33, 31, 370, DateTimeKind.Utc).AddTicks(4237) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 13, 16, 33, 31, 373, DateTimeKind.Utc).AddTicks(5880), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 13, 16, 33, 31, 373, DateTimeKind.Utc).AddTicks(5930) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalId",
                schema: "Company",
                table: "CompanyStructure");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 13, 16, 29, 42, 631, DateTimeKind.Utc).AddTicks(7216), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 13, 16, 29, 42, 632, DateTimeKind.Utc).AddTicks(882) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 13, 16, 29, 42, 635, DateTimeKind.Utc).AddTicks(5992), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 13, 16, 29, 42, 635, DateTimeKind.Utc).AddTicks(6045) });
        }
    }
}
