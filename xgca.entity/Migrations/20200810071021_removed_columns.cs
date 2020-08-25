using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class removed_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "Company",
                table: "Guest");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "Company",
                table: "Guest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Company",
                table: "Guest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                schema: "Company",
                table: "Guest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Company",
                table: "Guest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "IsDeleted",
                schema: "Company",
                table: "Guest",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                schema: "Company",
                table: "Guest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "Company",
                table: "Guest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
