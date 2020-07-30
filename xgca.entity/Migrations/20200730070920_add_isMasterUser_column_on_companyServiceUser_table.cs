using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class add_isMasterUser_column_on_companyServiceUser_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsMasterUser",
                schema: "Company",
                table: "CompanyServiceUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValue: new Guid("042cc05e-0fdc-48c1-9aa4-81bf12edf8bb"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("07d196ea-adea-4cf8-8790-0cb70542bbf8"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMasterUser",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("07d196ea-adea-4cf8-8790-0cb70542bbf8"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("042cc05e-0fdc-48c1-9aa4-81bf12edf8bb"));
        }
    }
}
