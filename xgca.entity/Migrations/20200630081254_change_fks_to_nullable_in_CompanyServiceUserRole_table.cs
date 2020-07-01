using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class change_fks_to_nullable_in_CompanyServiceUserRole_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceUser_CompanyServiceUserId",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceUserId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceRoleId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRole",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceUser_CompanyServiceUserId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceUserId",
                principalSchema: "Company",
                principalTable: "CompanyServiceUser",
                principalColumn: "CompanyServiceUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceUser_CompanyServiceUserId",
                schema: "Company",
                table: "CompanyServiceUserRole");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceUserId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceRoleId",
                principalSchema: "Company",
                principalTable: "CompanyServiceRole",
                principalColumn: "CompanyServiceRoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUserRole_CompanyServiceUser_CompanyServiceUserId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceUserId",
                principalSchema: "Company",
                principalTable: "CompanyServiceUser",
                principalColumn: "CompanyServiceUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
