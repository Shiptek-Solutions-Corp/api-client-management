using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class changed_column_datatype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "IsLocked",
                schema: "Company",
                table: "CompanyServiceUser",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "IsActive",
                schema: "Company",
                table: "CompanyServiceUser",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsLocked",
                schema: "Company",
                table: "CompanyServiceUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<int>(
                name: "IsActive",
                schema: "Company",
                table: "CompanyServiceUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte));
        }
    }
}
