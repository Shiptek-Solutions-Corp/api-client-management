using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class fixed_relationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyServiceId1",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUserRole_CompanyServiceId1",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceRole_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRole",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceUser_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyServiceUser",
                principalColumn: "CompanyServiceUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyService_CompanyServiceId1",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceId1",
                principalSchema: "Company",
                principalTable: "CompanyService",
                principalColumn: "CompanyServiceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceRole_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceUser_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyService_CompanyServiceId1",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropIndex(
                name: "IX_CompanyServiceUserRole_CompanyServiceId1",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropColumn(
                name: "CompanyServiceId1",
                schema: "Company",
                table: "CompanyServiceUserRole");
        }
    }
}
