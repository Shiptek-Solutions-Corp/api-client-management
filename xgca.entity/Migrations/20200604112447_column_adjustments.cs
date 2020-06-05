using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class column_adjustments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                schema: "General",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Address2",
                schema: "General",
                table: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceName",
                schema: "Services",
                table: "Service",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                schema: "Services",
                table: "Service",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine",
                schema: "General",
                table: "Address",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                schema: "General",
                table: "Address",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                schema: "General",
                table: "Address",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                schema: "Services",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "AddressLine",
                schema: "General",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "General",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "General",
                table: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceName",
                schema: "Services",
                table: "Service",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                schema: "General",
                table: "Address",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                schema: "General",
                table: "Address",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
