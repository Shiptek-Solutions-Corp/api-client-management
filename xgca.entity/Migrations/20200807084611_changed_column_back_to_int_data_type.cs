using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class changed_column_back_to_int_data_type : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_PreferredContact_CompanyId",
                schema: "Company",
                table: "PreferredContact");

            migrationBuilder.DropIndex(
                name: "IX_PreferredContact_GuestId",
                schema: "Company",
                table: "PreferredContact");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                schema: "Company",
                table: "PreferredContact",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                schema: "Company",
                table: "PreferredContact",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                schema: "Company",
                table: "PreferredContact",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                schema: "Company",
                table: "PreferredContact",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_PreferredContact_CompanyId",
                schema: "Company",
                table: "PreferredContact",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PreferredContact_GuestId",
                schema: "Company",
                table: "PreferredContact",
                column: "GuestId");

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
    }
}
