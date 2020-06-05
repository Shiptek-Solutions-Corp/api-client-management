using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class database_structure_adjustment_for_Auth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Services");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "Users",
                table: "User",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CompanyGroupResource",
                columns: table => new
                {
                    CompanyGroupResourceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyServiceRoleId = table.Column<int>(nullable: false),
                    ModuleGroupId = table.Column<int>(nullable: false),
                    IsAllowed = table.Column<byte>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyGroupResource", x => x.CompanyGroupResourceId);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                schema: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyService",
                schema: "Company",
                columns: table => new
                {
                    CompanyServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyService", x => x.CompanyServiceId);
                    table.ForeignKey(
                        name: "FK_CompanyService_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalSchema: "Services",
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyServiceRoles",
                schema: "Company",
                columns: table => new
                {
                    CompanyServiceRoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyServiceId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false),
                    CompanyGroupResourcesCompanyGroupResourceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyServiceRoles", x => x.CompanyServiceRoleId);
                    table.ForeignKey(
                        name: "FK_CompanyServiceRoles_CompanyGroupResource_CompanyGroupResourcesCompanyGroupResourceId",
                        column: x => x.CompanyGroupResourcesCompanyGroupResourceId,
                        principalTable: "CompanyGroupResource",
                        principalColumn: "CompanyGroupResourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyServiceRoles_CompanyService_CompanyServiceId",
                        column: x => x.CompanyServiceId,
                        principalSchema: "Company",
                        principalTable: "CompanyService",
                        principalColumn: "CompanyServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyServiceUser",
                schema: "Company",
                columns: table => new
                {
                    CompanyServiceUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyServiceId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UserTypeId = table.Column<int>(nullable: false),
                    CompanyServiceRoleId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyServiceUser", x => x.CompanyServiceUserId);
                    table.ForeignKey(
                        name: "FK_CompanyServiceUser_CompanyService_CompanyServiceId",
                        column: x => x.CompanyServiceId,
                        principalSchema: "Company",
                        principalTable: "CompanyService",
                        principalColumn: "CompanyServiceId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CompanyServiceUser_CompanyServiceRoles_CompanyServiceRoleId",
                        column: x => x.CompanyServiceRoleId,
                        principalSchema: "Company",
                        principalTable: "CompanyServiceRoles",
                        principalColumn: "CompanyServiceRoleId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CompanyServiceUser_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Users",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyService_CompanyId",
                schema: "Company",
                table: "CompanyService",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyService_ServiceId",
                schema: "Company",
                table: "CompanyService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceRoles_CompanyGroupResourcesCompanyGroupResourceId",
                schema: "Company",
                table: "CompanyServiceRoles",
                column: "CompanyGroupResourcesCompanyGroupResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceRoles_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRoles",
                column: "CompanyServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUser_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUser_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUser_UserId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyServiceUser",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "CompanyServiceRoles",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "CompanyGroupResource");

            migrationBuilder.DropTable(
                name: "CompanyService",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "Service",
                schema: "Services");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                schema: "Users",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "Users",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                schema: "Users",
                table: "User");
        }
    }
}
