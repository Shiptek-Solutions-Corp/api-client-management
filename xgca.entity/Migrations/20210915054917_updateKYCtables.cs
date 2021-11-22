using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class updateKYCtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyCodeFrom",
                schema: "Accreditation",
                table: "Request",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCodeTo",
                schema: "Accreditation",
                table: "Request",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyNameFrom",
                schema: "Accreditation",
                table: "Request",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyNameTo",
                schema: "Accreditation",
                table: "Request",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortCode",
                schema: "Accreditation",
                table: "PortArea",
                maxLength: 10,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 9, 15, 5, 49, 16, 743, DateTimeKind.Utc).AddTicks(3186), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 9, 15, 5, 49, 16, 743, DateTimeKind.Utc).AddTicks(4374) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 9, 15, 5, 49, 16, 744, DateTimeKind.Utc).AddTicks(9024), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 9, 15, 5, 49, 16, 744, DateTimeKind.Utc).AddTicks(9046) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyCodeFrom",
                schema: "Accreditation",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "CompanyCodeTo",
                schema: "Accreditation",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "CompanyNameFrom",
                schema: "Accreditation",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "CompanyNameTo",
                schema: "Accreditation",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "PortCode",
                schema: "Accreditation",
                table: "PortArea");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 8, 20, 3, 42, 17, 233, DateTimeKind.Utc).AddTicks(8255), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 8, 20, 3, 42, 17, 233, DateTimeKind.Utc).AddTicks(9227) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 8, 20, 3, 42, 17, 235, DateTimeKind.Utc).AddTicks(1428), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 8, 20, 3, 42, 17, 235, DateTimeKind.Utc).AddTicks(1447) });
        }
    }
}
