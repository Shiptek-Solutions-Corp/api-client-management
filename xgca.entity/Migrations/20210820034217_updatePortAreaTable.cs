using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class updatePortAreaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CityCode",
                schema: "Accreditation",
                table: "PortArea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                schema: "Accreditation",
                table: "PortArea",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                schema: "Accreditation",
                table: "PortArea",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                schema: "Accreditation",
                table: "PortArea",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                schema: "Accreditation",
                table: "PortArea",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "Accreditation",
                table: "PortArea",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Locode",
                schema: "Accreditation",
                table: "PortArea",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                schema: "Accreditation",
                table: "PortArea",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortName",
                schema: "Accreditation",
                table: "PortArea",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StateCode",
                schema: "Accreditation",
                table: "PortArea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateName",
                schema: "Accreditation",
                table: "PortArea",
                maxLength: 100,
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityCode",
                schema: "Accreditation",
                table: "PortArea");

            migrationBuilder.DropColumn(
                name: "CityName",
                schema: "Accreditation",
                table: "PortArea");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                schema: "Accreditation",
                table: "PortArea");

            migrationBuilder.DropColumn(
                name: "CountryName",
                schema: "Accreditation",
                table: "PortArea");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "Accreditation",
                table: "PortArea");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "Accreditation",
                table: "PortArea");

            migrationBuilder.DropColumn(
                name: "Locode",
                schema: "Accreditation",
                table: "PortArea");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "Accreditation",
                table: "PortArea");

            migrationBuilder.DropColumn(
                name: "PortName",
                schema: "Accreditation",
                table: "PortArea");

            migrationBuilder.DropColumn(
                name: "StateCode",
                schema: "Accreditation",
                table: "PortArea");

            migrationBuilder.DropColumn(
                name: "StateName",
                schema: "Accreditation",
                table: "PortArea");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 8, 16, 10, 45, 8, 523, DateTimeKind.Utc).AddTicks(8158), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 8, 16, 10, 45, 8, 523, DateTimeKind.Utc).AddTicks(9070) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 8, 16, 10, 45, 8, 525, DateTimeKind.Utc).AddTicks(460), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 8, 16, 10, 45, 8, 525, DateTimeKind.Utc).AddTicks(476) });
        }
    }
}
