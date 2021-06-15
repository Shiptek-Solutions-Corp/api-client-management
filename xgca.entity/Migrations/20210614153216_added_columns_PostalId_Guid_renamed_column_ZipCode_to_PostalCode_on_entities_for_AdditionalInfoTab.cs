using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_columns_PostalId_Guid_renamed_column_ZipCode_to_PostalCode_on_entities_for_AdditionalInfoTab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "CompanyDirectors",
                newName: "PostalCode",
                schema: "Company");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "CompanyBeneficialOwners",
                newName: "PostalCode",
                schema: "Company");

            migrationBuilder.AddColumn<string>(
                name: "PostalId",
                schema: "Company",
                table: "CompanyDirectors",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StateId",
                schema: "Company",
                table: "CompanyBeneficialOwners",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CityId",
                schema: "Company",
                table: "CompanyBeneficialOwners",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "PostalId",
                schema: "Company",
                table: "CompanyBeneficialOwners",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 14, 15, 32, 15, 930, DateTimeKind.Utc).AddTicks(7025), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 14, 15, 32, 15, 930, DateTimeKind.Utc).AddTicks(8403) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 14, 15, 32, 15, 932, DateTimeKind.Utc).AddTicks(6617), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 14, 15, 32, 15, 932, DateTimeKind.Utc).AddTicks(6670) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "CompanyDirectors",
                newName: "ZipCode",
                schema: "Company");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "CompanyBeneficialOwners",
                newName: "ZipCode",
                schema: "Company");

            migrationBuilder.DropColumn(
                name: "PostalId",
                schema: "Company",
                table: "CompanyDirectors");

            migrationBuilder.DropColumn(
                name: "PostalId",
                schema: "Company",
                table: "CompanyBeneficialOwners");

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

            migrationBuilder.AlterColumn<int>(
                name: "StateId",
                schema: "Company",
                table: "CompanyDirectors",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                schema: "Company",
                table: "CompanyDirectors",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StateId",
                schema: "Company",
                table: "CompanyBeneficialOwners",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                schema: "Company",
                table: "CompanyBeneficialOwners",
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
