using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_columns_on_CompanyServiceUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "Company",
                table: "CompanyServiceUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                schema: "Company",
                table: "CompanyServiceUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsLocked",
                schema: "Company",
                table: "CompanyServiceUser",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                schema: "Company",
                table: "CompanyServiceUser");
        }
    }
}
