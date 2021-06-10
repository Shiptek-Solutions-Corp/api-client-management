using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class AdditionalCompanyInfoTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KycStatusCode",
                schema: "Company",
                table: "Company",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyDirectors",
                schema: "Company",
                columns: table => new
                {
                    CompanyDirectorsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    CountryName = table.Column<string>(maxLength: 100, nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    StateName = table.Column<string>(maxLength: 100, nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 10, nullable: true),
                    CompanyAddress = table.Column<string>(maxLength: 1500, nullable: true),
                    AdditionalAddress = table.Column<string>(maxLength: 1500, nullable: true),
                    IsCompany = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false, defaultValueSql: "(N'ADMIN')"),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "(N'ADMIN')"),
                    UpdatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "(getutcdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 50, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDirectors", x => x.CompanyDirectorsId);
                    table.ForeignKey(
                        name: "FK_CompanyDirectors_Company",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyStructure",
                schema: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<int>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    RegistrationNumber = table.Column<string>(maxLength: 50, nullable: false),
                    TotalEmployeeNo = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    CountryName = table.Column<string>(maxLength: 100, nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    StateName = table.Column<string>(maxLength: 100, nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 10, nullable: true),
                    CompanyAddress = table.Column<string>(maxLength: 1500, nullable: true),
                    AdditionalAddress = table.Column<string>(maxLength: 1500, nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false, defaultValueSql: "(N'ADMIN')"),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "(N'ADMIN')"),
                    UpdatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "(getutcdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 50, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStructure", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_CompanyStructure_Company",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BeneficialOwnersType",
                schema: "Settings",
                columns: table => new
                {
                    BeneficialOwnersTypeCode = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false, defaultValueSql: "(N'ADMIN')"),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "(N'ADMIN')"),
                    UpdatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "(getutcdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 50, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeneficialOwnersType", x => x.BeneficialOwnersTypeCode);
                });

            migrationBuilder.CreateTable(
                name: "DocumentCategory",
                schema: "Settings",
                columns: table => new
                {
                    DocumentCategoryCode = table.Column<string>(maxLength: 10, nullable: false),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false, defaultValueSql: "(N'ADMIN')"),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "(N'ADMIN')"),
                    UpdatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "(getutcdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 50, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentCategory", x => x.DocumentCategoryCode);
                });

            migrationBuilder.CreateTable(
                name: "KycStatus",
                schema: "Settings",
                columns: table => new
                {
                    KycStatusCode = table.Column<string>(maxLength: 10, nullable: false),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    EnumIndex = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false, defaultValueSql: "(N'ADMIN')"),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "(N'ADMIN')"),
                    UpdatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "(getutcdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 50, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KycStatus", x => x.KycStatusCode);
                });

            migrationBuilder.CreateTable(
                name: "Section",
                schema: "Settings",
                columns: table => new
                {
                    SectionCode = table.Column<string>(maxLength: 10, nullable: false),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false, defaultValueSql: "(N'ADMIN')"),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "(N'ADMIN')"),
                    UpdatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "(getutcdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 50, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.SectionCode);
                });

            migrationBuilder.CreateTable(
                name: "SectionStatus",
                schema: "Settings",
                columns: table => new
                {
                    SectionStatusCode = table.Column<string>(maxLength: 10, nullable: false),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    EnumIndex = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false, defaultValueSql: "(N'ADMIN')"),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "(N'ADMIN')"),
                    UpdatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "(getutcdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 50, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionStatus", x => x.SectionStatusCode);
                });

            migrationBuilder.CreateTable(
                name: "CompanyBeneficialOwners",
                schema: "Company",
                columns: table => new
                {
                    CompanyBeneficialOwnersId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeneficialOwnersTypeCode = table.Column<string>(maxLength: 10, nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    CountryName = table.Column<string>(maxLength: 100, nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    StateName = table.Column<string>(maxLength: 100, nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 10, nullable: true),
                    CompanyAddress = table.Column<string>(maxLength: 1500, nullable: true),
                    AdditionalAddress = table.Column<string>(maxLength: 1500, nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false, defaultValueSql: "(N'ADMIN')"),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "(N'ADMIN')"),
                    UpdatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "(getutcdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 50, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyBeneficialOwners", x => x.CompanyBeneficialOwnersId);
                    table.ForeignKey(
                        name: "FK_CompanyBeneficialOwners_BeneficialOwnersType",
                        column: x => x.BeneficialOwnersTypeCode,
                        principalSchema: "Settings",
                        principalTable: "BeneficialOwnersType",
                        principalColumn: "BeneficialOwnersTypeCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyBeneficialOwners_Company",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentType",
                schema: "Settings",
                columns: table => new
                {
                    DocumentTypeCode = table.Column<string>(maxLength: 10, nullable: false),
                    DocumentCategoryCode = table.Column<string>(maxLength: 10, nullable: false),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false, defaultValueSql: "(N'ADMIN')"),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "(N'ADMIN')"),
                    UpdatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "(getutcdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 50, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentType", x => x.DocumentTypeCode);
                    table.ForeignKey(
                        name: "FK_DocumentType_DocumentCategory",
                        column: x => x.DocumentCategoryCode,
                        principalSchema: "Settings",
                        principalTable: "DocumentCategory",
                        principalColumn: "DocumentCategoryCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanySections",
                schema: "Company",
                columns: table => new
                {
                    CompanySectionsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    SectionStatusCode = table.Column<string>(maxLength: 10, nullable: false),
                    SectionCode = table.Column<string>(maxLength: 10, nullable: false),
                    IsDraft = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false, defaultValueSql: "(N'ADMIN')"),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "(N'ADMIN')"),
                    UpdatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "(getutcdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 50, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySections", x => x.CompanySectionsId);
                    table.ForeignKey(
                        name: "FK_CompanySections_Company",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanySections_Section",
                        column: x => x.SectionCode,
                        principalSchema: "Settings",
                        principalTable: "Section",
                        principalColumn: "SectionCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanySections_SectionStatus",
                        column: x => x.SectionStatusCode,
                        principalSchema: "Settings",
                        principalTable: "SectionStatus",
                        principalColumn: "SectionStatusCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDocuments",
                schema: "Company",
                columns: table => new
                {
                    CompanyDocumentsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    DocumentTypeCode = table.Column<string>(maxLength: 10, nullable: false),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    DocumentNo = table.Column<string>(maxLength: 50, nullable: true),
                    DocumentDescription = table.Column<byte[]>(maxLength: 550, nullable: true),
                    FileUrl = table.Column<string>(maxLength: 1500, nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false, defaultValueSql: "(N'ADMIN')"),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "(N'ADMIN')"),
                    UpdatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "(getutcdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 50, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDocuments", x => x.CompanyDocumentsId);
                    table.ForeignKey(
                        name: "FK_CompanyDocuments_Company",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyDocuments_DocumentType",
                        column: x => x.DocumentTypeCode,
                        principalSchema: "Settings",
                        principalTable: "DocumentType",
                        principalColumn: "DocumentTypeCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 10, 10, 14, 18, 131, DateTimeKind.Utc).AddTicks(4502), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 10, 10, 14, 18, 131, DateTimeKind.Utc).AddTicks(5548) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 10, 10, 14, 18, 132, DateTimeKind.Utc).AddTicks(7870), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 10, 10, 14, 18, 132, DateTimeKind.Utc).AddTicks(7891) });

            migrationBuilder.CreateIndex(
                name: "IX_Company_KycStatusCode",
                schema: "Company",
                table: "Company",
                column: "KycStatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBeneficialOwners_BeneficialOwnersTypeCode",
                schema: "Company",
                table: "CompanyBeneficialOwners",
                column: "BeneficialOwnersTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBeneficialOwners_CompanyId",
                schema: "Company",
                table: "CompanyBeneficialOwners",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDirectors_CompanyId",
                schema: "Company",
                table: "CompanyDirectors",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDocuments_CompanyId",
                schema: "Company",
                table: "CompanyDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDocuments_DocumentTypeCode",
                schema: "Company",
                table: "CompanyDocuments",
                column: "DocumentTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySections_SectionCode",
                schema: "Company",
                table: "CompanySections",
                column: "SectionCode");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySections_SectionStatusCode",
                schema: "Company",
                table: "CompanySections",
                column: "SectionStatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySections",
                schema: "Company",
                table: "CompanySections",
                columns: new[] { "CompanyId", "SectionCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentType",
                schema: "Settings",
                table: "DocumentType",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentType_DocumentCategoryCode",
                schema: "Settings",
                table: "DocumentType",
                column: "DocumentCategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_KycStatus",
                schema: "Settings",
                table: "KycStatus",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Section",
                schema: "Settings",
                table: "Section",
                columns: new[] { "SectionCode", "Description" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SectionStatus",
                schema: "Settings",
                table: "SectionStatus",
                column: "Description",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_KycStatus",
                schema: "Company",
                table: "Company",
                column: "KycStatusCode",
                principalSchema: "Settings",
                principalTable: "KycStatus",
                principalColumn: "KycStatusCode",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_KycStatus",
                schema: "Company",
                table: "Company");

            migrationBuilder.DropTable(
                name: "CompanyBeneficialOwners",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "CompanyDirectors",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "CompanyDocuments",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "CompanySections",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "CompanyStructure",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "KycStatus",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "BeneficialOwnersType",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "DocumentType",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Section",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "SectionStatus",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "DocumentCategory",
                schema: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Company_KycStatusCode",
                schema: "Company",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "KycStatusCode",
                schema: "Company",
                table: "Company");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 12, 2, 5, 2, 27, 352, DateTimeKind.Utc).AddTicks(7994), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 12, 2, 5, 2, 27, 352, DateTimeKind.Utc).AddTicks(8877) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 12, 2, 5, 2, 27, 353, DateTimeKind.Utc).AddTicks(9632), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 12, 2, 5, 2, 27, 353, DateTimeKind.Utc).AddTicks(9649) });
        }
    }
}
