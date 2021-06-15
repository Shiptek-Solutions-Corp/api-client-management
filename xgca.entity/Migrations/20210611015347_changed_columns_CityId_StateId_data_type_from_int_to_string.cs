using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class changed_columns_CityId_StateId_data_type_from_int_to_string : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StateId",
                schema: "Company",
                table: "CompanyStructure",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CityId",
                schema: "Company",
                table: "CompanyStructure",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 11, 1, 53, 46, 800, DateTimeKind.Utc).AddTicks(7679), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 11, 1, 53, 46, 800, DateTimeKind.Utc).AddTicks(8490) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 11, 1, 53, 46, 802, DateTimeKind.Utc).AddTicks(4478), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 11, 1, 53, 46, 802, DateTimeKind.Utc).AddTicks(4506) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StateId",
                schema: "Company",
                table: "CompanyStructure",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                schema: "Company",
                table: "CompanyStructure",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 10, 10, 14, 18, 131, DateTimeKind.Utc).AddTicks(4502), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 10, 10, 14, 18, 131, DateTimeKind.Utc).AddTicks(5548) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 10, 10, 14, 18, 132, DateTimeKind.Utc).AddTicks(7870), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 10, 10, 14, 18, 132, DateTimeKind.Utc).AddTicks(7891) });
        }
    }
}
