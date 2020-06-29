using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class update_schema_of_company_group_resource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "CompanyGroupResource",
                newName: "CompanyGroupResource",
                newSchema: "Company");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByName",
                schema: "Settings",
                table: "AuditLog",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByName",
                schema: "Settings",
                table: "AuditLog");

            migrationBuilder.RenameTable(
                name: "CompanyGroupResource",
                schema: "Company",
                newName: "CompanyGroupResource");
        }
    }
}
