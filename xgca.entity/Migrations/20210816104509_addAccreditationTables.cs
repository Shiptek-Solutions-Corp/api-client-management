using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class addAccreditationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Accreditation");

            migrationBuilder.CreateTable(
                name: "AccreditationStatusConfig",
                schema: "Accreditation",
                columns: table => new
                {
                    AccreditationStatusConfigId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Value = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditationStatusConfig", x => x.AccreditationStatusConfigId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRoleConfig",
                schema: "Accreditation",
                columns: table => new
                {
                    ServiceRoleConfigId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    ServiceRoleId = table.Column<Guid>(nullable: false),
                    ServiceRoleIdAllowed = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRoleConfig", x => x.ServiceRoleConfigId);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                schema: "Accreditation",
                columns: table => new
                {
                    RequestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccreditationStatusConfigId = table.Column<int>(nullable: false),
                    RequestStatusId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    ServiceRoleIdFrom = table.Column<Guid>(nullable: false),
                    CompanyIdFrom = table.Column<Guid>(nullable: false),
                    ServiceRoleIdTo = table.Column<Guid>(nullable: false),
                    CompanyIdTo = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_Request_AccreditationStatusConfig",
                        column: x => x.AccreditationStatusConfigId,
                        principalSchema: "Accreditation",
                        principalTable: "AccreditationStatusConfig",
                        principalColumn: "AccreditationStatusConfigId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PortArea",
                schema: "Accreditation",
                columns: table => new
                {
                    PortAreaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    CountryAreaId = table.Column<int>(nullable: false),
                    PortId = table.Column<Guid>(nullable: false),
                    PortOfLoading = table.Column<int>(nullable: false),
                    PortOfDischarge = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortArea", x => x.PortAreaId);
                    table.ForeignKey(
                        name: "FK_PortArea_Request",
                        column: x => x.RequestId,
                        principalSchema: "Accreditation",
                        principalTable: "Request",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TruckArea",
                schema: "Accreditation",
                columns: table => new
                {
                    TruckAreaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    CountryId = table.Column<int>(nullable: false),
                    CountryName = table.Column<string>(nullable: true),
                    StateId = table.Column<string>(nullable: true),
                    StateName = table.Column<string>(nullable: true),
                    CityId = table.Column<string>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
                    PostalId = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Latitude = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruckArea", x => x.TruckAreaId);
                    table.ForeignKey(
                        name: "FK_TruckArea_Request",
                        column: x => x.RequestId,
                        principalSchema: "Accreditation",
                        principalTable: "Request",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 8, 16, 10, 45, 8, 523, DateTimeKind.Utc).AddTicks(8158), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 8, 16, 10, 45, 8, 523, DateTimeKind.Utc).AddTicks(9070) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 8, 16, 10, 45, 8, 525, DateTimeKind.Utc).AddTicks(460), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 8, 16, 10, 45, 8, 525, DateTimeKind.Utc).AddTicks(476) });

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationStatusConfig",
                schema: "Accreditation",
                table: "AccreditationStatusConfig",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PortArea",
                schema: "Accreditation",
                table: "PortArea",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PortArea_RequestId",
                schema: "Accreditation",
                table: "PortArea",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_AccreditationStatusConfigId",
                schema: "Accreditation",
                table: "Request",
                column: "AccreditationStatusConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Request",
                schema: "Accreditation",
                table: "Request",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRoleConfig",
                schema: "Accreditation",
                table: "ServiceRoleConfig",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TruckArea",
                schema: "Accreditation",
                table: "TruckArea",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TruckArea_RequestId",
                schema: "Accreditation",
                table: "TruckArea",
                column: "RequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortArea",
                schema: "Accreditation");

            migrationBuilder.DropTable(
                name: "ServiceRoleConfig",
                schema: "Accreditation");

            migrationBuilder.DropTable(
                name: "TruckArea",
                schema: "Accreditation");

            migrationBuilder.DropTable(
                name: "Request",
                schema: "Accreditation");

            migrationBuilder.DropTable(
                name: "AccreditationStatusConfig",
                schema: "Accreditation");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 7, 9, 16, 26, 58, 181, DateTimeKind.Utc).AddTicks(1048), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 7, 9, 16, 26, 58, 181, DateTimeKind.Utc).AddTicks(2214) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 7, 9, 16, 26, 58, 182, DateTimeKind.Utc).AddTicks(8158), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 7, 9, 16, 26, 58, 182, DateTimeKind.Utc).AddTicks(8211) });
        }
    }
}
