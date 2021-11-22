using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_column_GUID_on_entities_CompanySections_CompanyDirectors_CompanyBeneficialOwners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanySections",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyDirectors",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyBeneficialOwners",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "Company",
                table: "CompanySections");

            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "Company",
                table: "CompanyDirectors");

            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "Company",
                table: "CompanyBeneficialOwners");

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
