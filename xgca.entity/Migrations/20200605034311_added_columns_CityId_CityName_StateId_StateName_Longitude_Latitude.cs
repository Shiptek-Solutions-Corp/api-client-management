using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_columns_CityId_CityName_StateId_StateName_Longitude_Latitude : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyService_Service_ServiceId",
                schema: "Company",
                table: "CompanyService");

            migrationBuilder.DropTable(
                name: "Service",
                schema: "Services");

            migrationBuilder.DropIndex(
                name: "IX_CompanyService_ServiceId",
                schema: "Company",
                table: "CompanyService");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "General",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "Landline",
                schema: "General",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "StateProvince",
                schema: "General",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "TownCity",
                schema: "General",
                table: "Address");

            migrationBuilder.AddColumn<string>(
                name: "FaxPrefix",
                schema: "General",
                table: "ContactDetail",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FaxPrefixId",
                schema: "General",
                table: "ContactDetail",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MobilePrefix",
                schema: "General",
                table: "ContactDetail",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MobilePrefixId",
                schema: "General",
                table: "ContactDetail",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                schema: "General",
                table: "ContactDetail",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhonePrefix",
                schema: "General",
                table: "ContactDetail",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhonePrefixId",
                schema: "General",
                table: "ContactDetail",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                schema: "General",
                table: "Address",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                schema: "General",
                table: "Address",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                schema: "General",
                table: "Address",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StateName",
                schema: "General",
                table: "Address",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaxPrefix",
                schema: "General",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "FaxPrefixId",
                schema: "General",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "MobilePrefix",
                schema: "General",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "MobilePrefixId",
                schema: "General",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "General",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "PhonePrefix",
                schema: "General",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "PhonePrefixId",
                schema: "General",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "CityId",
                schema: "General",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "CityName",
                schema: "General",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "StateId",
                schema: "General",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "StateName",
                schema: "General",
                table: "Address");

            migrationBuilder.EnsureSchema(
                name: "Services");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "General",
                table: "ContactDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Landline",
                schema: "General",
                table: "ContactDetail",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateProvince",
                schema: "General",
                table: "Address",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TownCity",
                schema: "General",
                table: "Address",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Service",
                schema: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ServiceId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyService_ServiceId",
                schema: "Company",
                table: "CompanyService",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyService_Service_ServiceId",
                schema: "Company",
                table: "CompanyService",
                column: "ServiceId",
                principalSchema: "Services",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
