using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class renamed_column_StatusCode_to_SectionStatusCode_on_entity_KYCLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusCode",
                table: "KYCLog",
                newName: "SectionStatusCode",
                schema: "Settings");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 7, 9, 15, 25, 43, 921, DateTimeKind.Utc).AddTicks(6440), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 7, 9, 15, 25, 43, 921, DateTimeKind.Utc).AddTicks(6948) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 7, 9, 15, 25, 43, 922, DateTimeKind.Utc).AddTicks(8623), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 7, 9, 15, 25, 43, 922, DateTimeKind.Utc).AddTicks(8657) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SectionStatusCode",
                table: "KYCLog",
                newName: "StatusCode",
                schema: "Settings");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 7, 7, 15, 39, 42, 74, DateTimeKind.Utc).AddTicks(7760), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 7, 7, 15, 39, 42, 74, DateTimeKind.Utc).AddTicks(8556) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 7, 7, 15, 39, 42, 76, DateTimeKind.Utc).AddTicks(3669), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 7, 7, 15, 39, 42, 76, DateTimeKind.Utc).AddTicks(3710) });
        }
    }
}
