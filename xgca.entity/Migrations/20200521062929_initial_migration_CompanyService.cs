using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class initial_migration_CompanyService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "General");

            migrationBuilder.EnsureSchema(
                name: "Settings");

            migrationBuilder.EnsureSchema(
                name: "Company");

            migrationBuilder.CreateTable(
                name: "ContactDetail",
                schema: "General",
                columns: table => new
                {
                    ContactDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Landline = table.Column<string>(maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true),
                    Fax = table.Column<string>(maxLength: 20, nullable: true),
                    IsDeleted = table.Column<byte>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDetail", x => x.ContactDetailId);
                });

            migrationBuilder.CreateTable(
                name: "AddressType",
                schema: "Settings",
                columns: table => new
                {
                    AddressTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressType", x => x.AddressTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "General",
                columns: table => new
                {
                    AddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressTypeId = table.Column<int>(nullable: false),
                    Address1 = table.Column<string>(maxLength: 100, nullable: true),
                    Address2 = table.Column<string>(maxLength: 100, nullable: true),
                    TownCity = table.Column<string>(maxLength: 100, nullable: true),
                    StateProvince = table.Column<string>(maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 10, nullable: true),
                    CountryId = table.Column<int>(nullable: false),
                    CountryName = table.Column<string>(maxLength: 100, nullable: false),
                    FullAddress = table.Column<string>(maxLength: 250, nullable: true),
                    IsDeleted = table.Column<byte>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Address_AddressType_AddressTypeId",
                        column: x => x.AddressTypeId,
                        principalSchema: "Settings",
                        principalTable: "AddressType",
                        principalColumn: "AddressTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 100, nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    ContactDetailId = table.Column<int>(nullable: false),
                    ImageURL = table.Column<string>(maxLength: 500, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: true),
                    WebsiteURL = table.Column<string>(maxLength: 100, nullable: true),
                    Status = table.Column<byte>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false),
                    TaxExemption = table.Column<byte>(nullable: false),
                    TaxExemptionStatus = table.Column<byte>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_Company_Address_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "General",
                        principalTable: "Address",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Company_ContactDetail_ContactDetailId",
                        column: x => x.ContactDetailId,
                        principalSchema: "General",
                        principalTable: "ContactDetail",
                        principalColumn: "ContactDetailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_AddressId",
                schema: "Company",
                table: "Company",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_ContactDetailId",
                schema: "Company",
                table: "Company",
                column: "ContactDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_AddressTypeId",
                schema: "General",
                table: "Address",
                column: "AddressTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "Address",
                schema: "General");

            migrationBuilder.DropTable(
                name: "ContactDetail",
                schema: "General");

            migrationBuilder.DropTable(
                name: "AddressType",
                schema: "Settings");
        }
    }
}
