using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class change_CompanyServiceRoleId_to_nullable_in_CompanyServiceUser_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUser_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceRole_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUser_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceRoleId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRole",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceRole_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRole",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUser_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceRole_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUser_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceRoleId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRole",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceRole_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRole",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
