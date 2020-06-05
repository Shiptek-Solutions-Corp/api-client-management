using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_entity_CompanyUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceRoles_CompanyGroupResource_CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceRoles_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUser_CompanyServiceRoles_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUser_Company_CompaniesCompanyId",
                schema: "Company",
                table: "CompanyUser");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUser_CompaniesCompanyId",
                schema: "Company",
                table: "CompanyUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyServiceRoles",
                schema: "Company",
                table: "CompanyServiceRoles");

            migrationBuilder.DropColumn(
                name: "CompaniesCompanyId",
                schema: "Company",
                table: "CompanyUser");

            migrationBuilder.DropColumn(
                name: "ConmpanyId",
                schema: "Company",
                table: "CompanyUser");

            migrationBuilder.RenameTable(
                name: "CompanyServiceRoles",
                schema: "Company",
                newName: "CompanyServiceRole",
                newSchema: "Company");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyServiceRoles_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRole",
                newName: "IX_CompanyServiceRole_CompanyServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyServiceRoles_CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRole",
                newName: "IX_CompanyServiceRole_CompanyGroupResourcesCompanyGroupResourceId");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "Company",
                table: "CompanyUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyServiceRole",
                schema: "Company",
                table: "CompanyServiceRole",
                column: "CompanyServiceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_CompanyId",
                schema: "Company",
                table: "CompanyUser",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceRole_CompanyGroupResource_CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRole",
                column: "CompanyGroupResourcesCompanyGroupResourceId",
                principalTable: "CompanyGroupResource",
                principalColumn: "CompanyGroupResourceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceRole_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRole",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyService",
                principalColumn: "CompanyServiceId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUser_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceRoleId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRole",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUser_Company_CompanyId",
                schema: "Company",
                table: "CompanyUser",
                column: "CompanyId",
                principalSchema: "Company",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceRole_CompanyGroupResource_CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRole");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceRole_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRole");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUser_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUser_Company_CompanyId",
                schema: "Company",
                table: "CompanyUser");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUser_CompanyId",
                schema: "Company",
                table: "CompanyUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyServiceRole",
                schema: "Company",
                table: "CompanyServiceRole");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "Company",
                table: "CompanyUser");

            migrationBuilder.RenameTable(
                name: "CompanyServiceRole",
                schema: "Company",
                newName: "CompanyServiceRoles",
                newSchema: "Company");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyServiceRole_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRoles",
                newName: "IX_CompanyServiceRoles_CompanyServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyServiceRole_CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRoles",
                newName: "IX_CompanyServiceRoles_CompanyGroupResourcesCompanyGroupResourceId");

            migrationBuilder.AddColumn<int>(
                name: "CompaniesCompanyId",
                schema: "Company",
                table: "CompanyUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConmpanyId",
                schema: "Company",
                table: "CompanyUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyServiceRoles",
                schema: "Company",
                table: "CompanyServiceRoles",
                column: "CompanyServiceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_CompaniesCompanyId",
                schema: "Company",
                table: "CompanyUser",
                column: "CompaniesCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceRoles_CompanyGroupResource_CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRoles",
                column: "CompanyGroupResourcesCompanyGroupResourceId",
                principalTable: "CompanyGroupResource",
                principalColumn: "CompanyGroupResourceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceRoles_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRoles",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyService",
                principalColumn: "CompanyServiceId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUser_CompanyServiceRoles_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceRoleId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRoles",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUser_Company_CompaniesCompanyId",
                schema: "Company",
                table: "CompanyUser",
                column: "CompaniesCompanyId",
                principalSchema: "Company",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
