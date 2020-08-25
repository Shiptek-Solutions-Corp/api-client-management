using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class changed_column_to_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreferredContact_Company_CompanyId",
                schema: "Company",
                table: "PreferredContact");

            migrationBuilder.DropForeignKey(
                name: "FK_PreferredContact_Guest_GuestId",
                schema: "Company",
                table: "PreferredContact");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                schema: "Company",
                table: "PreferredContact",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                schema: "Company",
                table: "PreferredContact",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PreferredContact_Company_CompanyId",
                schema: "Company",
                table: "PreferredContact",
                column: "CompanyId",
                principalSchema: "Company",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreferredContact_Guest_GuestId",
                schema: "Company",
                table: "PreferredContact",
                column: "GuestId",
                principalSchema: "Company",
                principalTable: "Guest",
                principalColumn: "GuestId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreferredContact_Company_CompanyId",
                schema: "Company",
                table: "PreferredContact");

            migrationBuilder.DropForeignKey(
                name: "FK_PreferredContact_Guest_GuestId",
                schema: "Company",
                table: "PreferredContact");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                schema: "Company",
                table: "PreferredContact",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                schema: "Company",
                table: "PreferredContact",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PreferredContact_Company_CompanyId",
                schema: "Company",
                table: "PreferredContact",
                column: "CompanyId",
                principalSchema: "Company",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreferredContact_Guest_GuestId",
                schema: "Company",
                table: "PreferredContact",
                column: "GuestId",
                principalSchema: "Company",
                principalTable: "Guest",
                principalColumn: "GuestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
