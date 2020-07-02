using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class removed_unecessary_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "MenuModule",
                schema: "Module");

            migrationBuilder.DropTable(
                name: "ModuleGroup",
                schema: "Module");

            migrationBuilder.DropIndex(
                name: "IX_CompanyGroupResource_GroupResourceId",
                schema: "Company",
                table: "CompanyGroupResource");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyGroupResource_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource",
                column: "CompanyServiceRoleId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRole",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyGroupResource_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource");

            migrationBuilder.EnsureSchema(
                name: "Module");

            migrationBuilder.CreateTable(
                name: "MenuModule",
                schema: "Module",
                columns: table => new
                {
                    MenuModulesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    SubMenuId = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuModule", x => x.MenuModulesId);
                });

            migrationBuilder.CreateTable(
                name: "ModuleGroup",
                schema: "Module",
                columns: table => new
                {
                    ModuleGroupsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    ResourceGroupId = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    GroupResourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModuleGroupId = table.Column<int>(type: "int", nullable: false),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                onDelete: ReferentialAction.Restrict);

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
    }
}
