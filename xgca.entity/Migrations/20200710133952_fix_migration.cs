using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class fix_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUser_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUser_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUser_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyService",
                principalColumn: "CompanyServiceId",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUser_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceRoleId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRole",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUser_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUser_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUser_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyService",
                principalColumn: "CompanyServiceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUser_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceRoleId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRole",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
