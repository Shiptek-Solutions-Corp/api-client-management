using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class rename_ZipCode_to_PostalCode_on_entity_CompanyStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "CompanyStructure",
                newName: "PostalCode",
                schema: "Company");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "CompanyStructure",
                newName: "ZipCode",
                schema: "Company");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 13, 16, 9, 17, 550, DateTimeKind.Utc).AddTicks(1433), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 13, 16, 9, 17, 550, DateTimeKind.Utc).AddTicks(3094) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 13, 16, 9, 17, 552, DateTimeKind.Utc).AddTicks(2559), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 13, 16, 9, 17, 552, DateTimeKind.Utc).AddTicks(2626) });
        }
    }
}
