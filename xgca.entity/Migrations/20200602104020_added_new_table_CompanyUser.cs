using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_new_table_CompanyUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUser_User_UserId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropIndex(
                name: "IX_CompanyServiceUser_UserId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.AddColumn<int>(
                name: "CompanyUserId",
                schema: "Company",
                table: "CompanyServiceUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CompanyUser",
                schema: "Company",
                columns: table => new
                {
                    CompanyUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConmpanyId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UserTypeId = table.Column<int>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    CompaniesCompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUser", x => x.CompanyUserId);
                    table.ForeignKey(
                        name: "FK_CompanyUser_Company_CompaniesCompanyId",
                        column: x => x.CompaniesCompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyUser_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Users",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUser_CompanyUserId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_CompaniesCompanyId",
                schema: "Company",
                table: "CompanyUser",
                column: "CompaniesCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_UserId",
                schema: "Company",
                table: "CompanyUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUser_CompanyUser_CompanyUserId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyUserId",
                principalSchema: "Company",
                principalTable: "CompanyUser",
                principalColumn: "CompanyUserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUser_CompanyUser_CompanyUserId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropTable(
                name: "CompanyUser",
                schema: "Company");

            migrationBuilder.DropIndex(
                name: "IX_CompanyServiceUser_CompanyUserId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropColumn(
                name: "CompanyUserId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "Company",
                table: "CompanyServiceUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                schema: "Company",
                table: "CompanyServiceUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUser_UserId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUser_User_UserId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
