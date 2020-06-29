using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class Added_new_migrations_based_on_acm_structure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceRole_CompanyGroupResource_CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRole");

            migrationBuilder.DropIndex(
                name: "IX_CompanyServiceRole_CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRole");

            migrationBuilder.DropColumn(
                name: "CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRole");

            migrationBuilder.DropColumn(
                name: "ModuleGroupId",
                schema: "Company",
                table: "CompanyGroupResource");

            migrationBuilder.EnsureSchema(
                name: "Module");

            migrationBuilder.AddColumn<int>(
                name: "GroupResourceId",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ModuleGroup",
                schema: "Module",
                columns: table => new
                {
                    ModuleGroupsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    ResourceGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleGroup", x => x.ModuleGroupsId);
                });

            migrationBuilder.CreateTable(
                name: "GroupResource",
                schema: "Module",
                columns: table => new
                {
                    GroupResourceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    ModuleGroupId = table.Column<int>(nullable: false),
                    ResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupResource", x => x.GroupResourceId);
                    table.ForeignKey(
                        name: "FK_GroupResource_ModuleGroup_ModuleGroupId",
                        column: x => x.ModuleGroupId,
                        principalSchema: "Module",
                        principalTable: "ModuleGroup",
                        principalColumn: "ModuleGroupsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyGroupResource_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource",
                column: "CompanyServiceRoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyGroupResource_GroupResourceId",
                schema: "Company",
                table: "CompanyGroupResource",
                column: "GroupResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupResource_ModuleGroupId",
                schema: "Module",
                table: "GroupResource",
                column: "ModuleGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyGroupResource_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource",
                column: "CompanyServiceRoleId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRole",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyGroupResource_GroupResource_GroupResourceId",
                schema: "Company",
                table: "CompanyGroupResource",
                column: "GroupResourceId",
                principalSchema: "Module",
                principalTable: "GroupResource",
                principalColumn: "GroupResourceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyGroupResource_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyGroupResource_GroupResource_GroupResourceId",
                schema: "Company",
                table: "CompanyGroupResource");

            migrationBuilder.DropTable(
                name: "GroupResource",
                schema: "Module");

            migrationBuilder.DropTable(
                name: "ModuleGroup",
                schema: "Module");

            migrationBuilder.DropIndex(
                name: "IX_CompanyGroupResource_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource");

            migrationBuilder.DropIndex(
                name: "IX_CompanyGroupResource_GroupResourceId",
                schema: "Company",
                table: "CompanyGroupResource");

            migrationBuilder.DropColumn(
                name: "GroupResourceId",
                schema: "Company",
                table: "CompanyGroupResource");

            migrationBuilder.AddColumn<int>(
                name: "CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRole",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModuleGroupId",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceRole_CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRole",
                column: "CompanyGroupResourcesCompanyGroupResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceRole_CompanyGroupResource_CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRole",
                column: "CompanyGroupResourcesCompanyGroupResourceId",
                principalSchema: "Company",
                principalTable: "CompanyGroupResource",
                principalColumn: "CompanyGroupResourceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
