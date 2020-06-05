using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_column_Guid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "Settings",
                table: "AuditLog",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "Settings",
                table: "AddressType",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "General",
                table: "ContactDetail",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "General",
                table: "Address",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "Company",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "Settings",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "Settings",
                table: "AddressType");

            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "General",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "General",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "Company",
                table: "Company");
        }
    }
}
