using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class changed_column_data_type_of_TotalEmployeeNo_from_int_to_string : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TotalEmployeeNo",
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
                values: new object[] { new DateTime(2021, 6, 13, 16, 6, 31, 219, DateTimeKind.Utc).AddTicks(9861), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 13, 16, 6, 31, 220, DateTimeKind.Utc).AddTicks(1721) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 13, 16, 6, 31, 222, DateTimeKind.Utc).AddTicks(1686), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 13, 16, 6, 31, 222, DateTimeKind.Utc).AddTicks(1721) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TotalEmployeeNo",
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
                values: new object[] { new DateTime(2021, 6, 13, 15, 37, 41, 284, DateTimeKind.Utc).AddTicks(3760), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 13, 15, 37, 41, 284, DateTimeKind.Utc).AddTicks(5871) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 13, 15, 37, 41, 287, DateTimeKind.Utc).AddTicks(3156), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 13, 15, 37, 41, 287, DateTimeKind.Utc).AddTicks(3484) });
        }
    }
}
