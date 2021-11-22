using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class change_data_type_of_column_DocumentDescription_from_byte_to_string_on_entity_CompanyDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DocumentDescription",
                schema: "Company",
                table: "CompanyDocuments",
                maxLength: 550,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(550)",
                oldMaxLength: 550,
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 14, 16, 35, 51, 90, DateTimeKind.Utc).AddTicks(5800), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 14, 16, 35, 51, 90, DateTimeKind.Utc).AddTicks(6635) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 14, 16, 35, 51, 91, DateTimeKind.Utc).AddTicks(8596), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 14, 16, 35, 51, 91, DateTimeKind.Utc).AddTicks(8614) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "DocumentDescription",
                schema: "Company",
                table: "CompanyDocuments",
                type: "varbinary(550)",
                maxLength: 550,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 550,
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 14, 15, 59, 42, 753, DateTimeKind.Utc).AddTicks(3504), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 14, 15, 59, 42, 753, DateTimeKind.Utc).AddTicks(5469) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 14, 15, 59, 42, 755, DateTimeKind.Utc).AddTicks(9480), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 14, 15, 59, 42, 755, DateTimeKind.Utc).AddTicks(9518) });
        }
    }
}
