using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class change_companygroupresource_relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyGroupResource_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole");

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

            migrationBuilder.DropIndex(
                name: "IX_CompanyGroupResource_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource");

            migrationBuilder.DropColumn(
                name: "CompanyServiceId1",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyGroupResource_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource",
                column: "CompanyServiceRoleId");

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
                name: "FK_CompanyServiceUserRole_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyService",
                principalColumn: "CompanyServiceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyGroupResource_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropIndex(
                name: "IX_CompanyGroupResource_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource");

            migrationBuilder.AddColumn<int>(
                name: "CompanyServiceId1",
                schema: "Company",
                table: "CompanyServiceUserRole",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUserRole_CompanyServiceId1",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceId1");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyGroupResource_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource",
                column: "CompanyServiceRoleId",
                unique: true);

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
                name: "FK_CompanyServiceUserRole_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyService",
                principalColumn: "CompanyServiceId",
                onDelete: ReferentialAction.Cascade);

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
                onDelete: ReferentialAction.Cascade);

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
    }
}
