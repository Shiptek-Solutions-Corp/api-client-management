using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_new_column_for_address : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressAdditionalInformation",
                schema: "General",
                table: "Address",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("2c609a59-407e-4dce-8bee-f17f8e00ce03"));

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 12, 2, 5, 2, 27, 352, DateTimeKind.Utc).AddTicks(7994), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 12, 2, 5, 2, 27, 352, DateTimeKind.Utc).AddTicks(8877) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 12, 2, 5, 2, 27, 353, DateTimeKind.Utc).AddTicks(9632), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 12, 2, 5, 2, 27, 353, DateTimeKind.Utc).AddTicks(9649) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressAdditionalInformation",
                schema: "General",
                table: "Address");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("2c609a59-407e-4dce-8bee-f17f8e00ce03"),
                oldClrType: typeof(Guid),
                oldDefaultValueSql: "NEWID()");

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
    }
}
