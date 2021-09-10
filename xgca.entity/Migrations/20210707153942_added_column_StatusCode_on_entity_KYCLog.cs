using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_column_StatusCode_on_entity_KYCLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusCode",
                schema: "Settings",
                table: "KYCLog",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusCode",
                schema: "Settings",
                table: "KYCLog");

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
    }
}
