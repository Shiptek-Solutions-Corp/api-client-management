using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class initial_migration_Guest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsDeleted",
                schema: "Company",
                table: "CompanyServiceUserRole",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValue: new Guid("acf85556-f111-4f58-a196-b64bcef3b1cb"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("042cc05e-0fdc-48c1-9aa4-81bf12edf8bb"));

            migrationBuilder.CreateTable(
                name: "Guest",
                schema: "Company",
                columns: table => new
                {
                    GuestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestType = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    GuestName = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    PhoneNumberPrefix = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    MobileNumberPrefix = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    FaxNumberPrefix = table.Column<string>(nullable: true),
                    FaxNumber = table.Column<string>(nullable: true),
                    AddressLine = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    ProvinceState = table.Column<string>(nullable: true),
                    CityMunicipality = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    IsGuest = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<int>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guest", x => x.GuestId);
                    table.ForeignKey(
                        name: "FK_Guest_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guest_CompanyId",
                schema: "Company",
                table: "Guest",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guest",
                schema: "Company");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "Company",
                table: "CompanyServiceUserRole",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("042cc05e-0fdc-48c1-9aa4-81bf12edf8bb"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("acf85556-f111-4f58-a196-b64bcef3b1cb"));
        }
    }
}
