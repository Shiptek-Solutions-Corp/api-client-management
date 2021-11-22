using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_column_Guid_on_entity_KYCLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "Settings",
                table: "KYCLog",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 7, 7, 14, 26, 48, 301, DateTimeKind.Utc).AddTicks(5369), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 7, 7, 14, 26, 48, 301, DateTimeKind.Utc).AddTicks(6229) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 7, 7, 14, 26, 48, 303, DateTimeKind.Utc).AddTicks(1765), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 7, 7, 14, 26, 48, 303, DateTimeKind.Utc).AddTicks(1798) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "Settings",
                table: "KYCLog");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 7, 7, 13, 57, 42, 329, DateTimeKind.Utc).AddTicks(509), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 7, 7, 13, 57, 42, 329, DateTimeKind.Utc).AddTicks(1523) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 7, 7, 13, 57, 42, 331, DateTimeKind.Utc).AddTicks(90), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 7, 7, 13, 57, 42, 331, DateTimeKind.Utc).AddTicks(124) });
        }
    }
}
