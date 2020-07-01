using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class Added_new_migrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyServiceUserRole",
                schema: "Company",
                columns: table => new
                {
                    CompanyServiceUserRoleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyServiceId = table.Column<int>(nullable: false),
                    CompanyServiceUserId = table.Column<int>(nullable: false),
                    CompanyServiceRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyServiceUserRole", x => x.CompanyServiceUserRoleID);
                    table.ForeignKey(
                        name: "FK_CompanyServiceUserRole_CompanyService_CompanyServiceId",
                        column: x => x.CompanyServiceId,
                        principalSchema: "Company",
                        principalTable: "CompanyService",
                        principalColumn: "CompanyServiceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyServiceUserRole_CompanyServiceRole_CompanyServiceRoleId",
                        column: x => x.CompanyServiceRoleId,
                        principalSchema: "Company",
                        principalTable: "CompanyServiceRole",
                        principalColumn: "CompanyServiceRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyServiceUserRole_CompanyServiceUser_CompanyServiceUserId",
                        column: x => x.CompanyServiceUserId,
                        principalSchema: "Company",
                        principalTable: "CompanyServiceUser",
                        principalColumn: "CompanyServiceUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuModule",
                schema: "Module",
                columns: table => new
                {
                    MenuModulesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    MenuId = table.Column<int>(nullable: false),
                    SubMenuId = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuModule", x => x.MenuModulesId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUserRole_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUserRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUserRole_CompanyServiceUserId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyServiceUserRole",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "MenuModule",
                schema: "Module");
        }
    }
}
