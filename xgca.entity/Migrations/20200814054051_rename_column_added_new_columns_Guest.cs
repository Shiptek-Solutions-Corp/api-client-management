using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class rename_column_added_new_columns_Guest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityMunicipality",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "ProvinceState",
                schema: "Company",
                table: "Guest");

            migrationBuilder.AddColumn<string>(
                name: "CityId",
                schema: "Company",
                table: "Guest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                schema: "Company",
                table: "Guest",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                schema: "Company",
                table: "Guest",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                schema: "Company",
                table: "Guest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaxNumberPrefixId",
                schema: "Company",
                table: "Guest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileNumberPrefixId",
                schema: "Company",
                table: "Guest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberPrefixId",
                schema: "Company",
                table: "Guest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateId",
                schema: "Company",
                table: "Guest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateName",
                schema: "Company",
                table: "Guest",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "CityName",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "CountryId",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "CountryName",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "FaxNumberPrefixId",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "MobileNumberPrefixId",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "PhoneNumberPrefixId",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "StateId",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "StateName",
                schema: "Company",
                table: "Guest");

            migrationBuilder.AddColumn<string>(
                name: "CityMunicipality",
                schema: "Company",
                table: "Guest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "Company",
                table: "Guest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceState",
                schema: "Company",
                table: "Guest",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
