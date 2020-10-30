using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class changed_column_length_User_Username : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                schema: "Users",
                table: "User",
                maxLength: 24,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 10, 30, 10, 14, 31, 840, DateTimeKind.Utc).AddTicks(7086), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 10, 30, 10, 14, 31, 840, DateTimeKind.Utc).AddTicks(8830) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 10, 30, 10, 14, 31, 844, DateTimeKind.Utc).AddTicks(714), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 10, 30, 10, 14, 31, 844, DateTimeKind.Utc).AddTicks(755) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                schema: "Users",
                table: "User",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 24,
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 10, 27, 6, 37, 46, 96, DateTimeKind.Utc).AddTicks(5368), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 10, 27, 6, 37, 46, 96, DateTimeKind.Utc).AddTicks(7262) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 10, 27, 6, 37, 46, 99, DateTimeKind.Utc).AddTicks(3869), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 10, 27, 6, 37, 46, 99, DateTimeKind.Utc).AddTicks(3921) });
        }
    }
}
