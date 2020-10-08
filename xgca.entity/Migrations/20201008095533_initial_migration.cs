using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class initial_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "General");

            migrationBuilder.EnsureSchema(
                name: "Settings");

            migrationBuilder.EnsureSchema(
                name: "Company");

            migrationBuilder.EnsureSchema(
                name: "Users");

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
                    PhoneNumberPrefixId = table.Column<string>(nullable: true),
                    PhoneNumberPrefix = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    MobileNumberPrefixId = table.Column<string>(nullable: true),
                    MobileNumberPrefix = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    FaxNumberPrefixId = table.Column<string>(nullable: true),
                    FaxNumberPrefix = table.Column<string>(nullable: true),
                    FaxNumber = table.Column<string>(nullable: true),
                    AddressLine = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: false),
                    CountryName = table.Column<string>(nullable: true),
                    StateId = table.Column<string>(nullable: true),
                    StateName = table.Column<string>(nullable: true),
                    CityId = table.Column<string>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    IsGuest = table.Column<bool>(nullable: false),
                    CUCC = table.Column<string>(nullable: true),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guest", x => x.GuestId);
                });

            migrationBuilder.CreateTable(
                name: "PreferredContact",
                schema: "Company",
                columns: table => new
                {
                    PreferredContactId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestId = table.Column<string>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    ProfileId = table.Column<int>(nullable: false),
                    ContactType = table.Column<int>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<int>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferredContact", x => x.PreferredContactId);
                });

            migrationBuilder.CreateTable(
                name: "PreferredProvider",
                schema: "Company",
                columns: table => new
                {
                    PreferredProviderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyServiceId = table.Column<string>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    ServiceId = table.Column<string>(nullable: true),
                    ProfileId = table.Column<int>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<int>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferredProvider", x => x.PreferredProviderId);
                });

            migrationBuilder.CreateTable(
                name: "ContactDetail",
                schema: "General",
                columns: table => new
                {
                    ContactDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhonePrefixId = table.Column<int>(nullable: false),
                    PhonePrefix = table.Column<string>(maxLength: 10, nullable: true),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    MobilePrefixId = table.Column<int>(nullable: false),
                    MobilePrefix = table.Column<string>(maxLength: 10, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true),
                    FaxPrefixId = table.Column<int>(nullable: false),
                    FaxPrefix = table.Column<string>(maxLength: 10, nullable: true),
                    Fax = table.Column<string>(maxLength: 20, nullable: true),
                    IsDeleted = table.Column<byte>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false)
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
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressType", x => x.AddressTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                schema: "Settings",
                columns: table => new
                {
                    AuditLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditLogAction = table.Column<string>(maxLength: 20, nullable: true),
                    TableName = table.Column<string>(maxLength: 50, nullable: true),
                    KeyFieldId = table.Column<int>(nullable: false),
                    NewValue = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedByName = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.AuditLogId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(maxLength: 20, nullable: true),
                    ContactDetailId = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 100, nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: true),
                    ImageURL = table.Column<string>(maxLength: 500, nullable: true),
                    Status = table.Column<byte>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    IsLocked = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_ContactDetail_ContactDetailId",
                        column: x => x.ContactDetailId,
                        principalSchema: "General",
                        principalTable: "ContactDetail",
                        principalColumn: "ContactDetailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "General",
                columns: table => new
                {
                    AddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressTypeId = table.Column<int>(nullable: false),
                    AddressLine = table.Column<string>(maxLength: 255, nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(maxLength: 100, nullable: true),
                    StateId = table.Column<int>(nullable: false),
                    StateName = table.Column<string>(maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 10, nullable: true),
                    CountryId = table.Column<int>(nullable: false),
                    CountryName = table.Column<string>(maxLength: 100, nullable: false),
                    FullAddress = table.Column<string>(maxLength: 250, nullable: true),
                    IsDeleted = table.Column<byte>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    Longitude = table.Column<string>(maxLength: 50, nullable: true),
                    Latitude = table.Column<string>(maxLength: 50, nullable: true)
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
                    ClientId = table.Column<int>(nullable: false),
                    CompanyCode = table.Column<string>(maxLength: 10, nullable: true),
                    CompanyName = table.Column<string>(maxLength: 160, nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    ContactDetailId = table.Column<int>(nullable: false),
                    ImageURL = table.Column<string>(maxLength: 500, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 74, nullable: true),
                    WebsiteURL = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<byte>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false),
                    TaxExemption = table.Column<byte>(nullable: false),
                    TaxExemptionStatus = table.Column<byte>(nullable: false),
                    CUCC = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    AccreditedBy = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "CompanyService",
                schema: "Company",
                columns: table => new
                {
                    CompanyServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false),
                    Status = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyService", x => x.CompanyServiceId);
                    table.ForeignKey(
                        name: "FK_CompanyService_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyUser",
                schema: "Company",
                columns: table => new
                {
                    CompanyUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UserTypeId = table.Column<int>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUser", x => x.CompanyUserId);
                    table.ForeignKey(
                        name: "FK_CompanyUser_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CompanyUser_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Users",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Invite",
                schema: "Company",
                columns: table => new
                {
                    InviteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InviteCode = table.Column<string>(nullable: true),
                    ReceiverId = table.Column<string>(nullable: true),
                    ReceiverEmail = table.Column<string>(nullable: true),
                    InviteType = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ExpiresOn = table.Column<DateTime>(nullable: false),
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invite", x => x.InviteId);
                    table.ForeignKey(
                        name: "FK_Invite_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyServiceRole",
                schema: "Company",
                columns: table => new
                {
                    CompanyServiceRoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyServiceId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false),
                    IsActive = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyServiceRole", x => x.CompanyServiceRoleId);
                    table.ForeignKey(
                        name: "FK_CompanyServiceRole_CompanyService_CompanyServiceId",
                        column: x => x.CompanyServiceId,
                        principalSchema: "Company",
                        principalTable: "CompanyService",
                        principalColumn: "CompanyServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyGroupResource",
                schema: "Company",
                columns: table => new
                {
                    CompanyGroupResourceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyServiceRoleId = table.Column<int>(nullable: false),
                    GroupResourceId = table.Column<int>(nullable: false),
                    IsAllowed = table.Column<byte>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedBy = table.Column<int>(nullable: false, defaultValue: 1),
                    ModifiedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    Guid = table.Column<Guid>(nullable: false, defaultValue: new Guid("e2a63825-ceda-4a87-b643-f2e211150318")),
                    IsDeleted = table.Column<byte>(nullable: false, defaultValue: (byte)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyGroupResource", x => x.CompanyGroupResourceId);
                    table.ForeignKey(
                        name: "FK_CompanyGroupResource_CompanyServiceRole_CompanyServiceRoleId",
                        column: x => x.CompanyServiceRoleId,
                        principalSchema: "Company",
                        principalTable: "CompanyServiceRole",
                        principalColumn: "CompanyServiceRoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyServiceUser",
                schema: "Company",
                columns: table => new
                {
                    CompanyServiceUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyServiceId = table.Column<int>(nullable: false),
                    CompanyUserId = table.Column<int>(nullable: false),
                    CompanyServiceRoleId = table.Column<int>(nullable: false),
                    IsMasterUser = table.Column<int>(nullable: false, defaultValue: 0),
                    CompanyId = table.Column<int>(nullable: false),
                    IsActive = table.Column<byte>(nullable: false),
                    IsLocked = table.Column<byte>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyServiceUser", x => x.CompanyServiceUserId);
                    table.ForeignKey(
                        name: "FK_CompanyServiceUser_CompanyService_CompanyServiceId",
                        column: x => x.CompanyServiceId,
                        principalSchema: "Company",
                        principalTable: "CompanyService",
                        principalColumn: "CompanyServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyServiceUser_CompanyServiceRole_CompanyServiceRoleId",
                        column: x => x.CompanyServiceRoleId,
                        principalSchema: "Company",
                        principalTable: "CompanyServiceRole",
                        principalColumn: "CompanyServiceRoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyServiceUser_CompanyUser_CompanyUserId",
                        column: x => x.CompanyUserId,
                        principalSchema: "Company",
                        principalTable: "CompanyUser",
                        principalColumn: "CompanyUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyServiceUserRole",
                schema: "Company",
                columns: table => new
                {
                    CompanyServiceUserRoleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<int>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CompanyServiceId = table.Column<int>(nullable: true),
                    CompanyServiceUserId = table.Column<int>(nullable: true),
                    CompanyServiceRoleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyServiceUserRole", x => x.CompanyServiceUserRoleID);
                    table.ForeignKey(
                        name: "FK_CompanyServiceUserRole_CompanyService_CompanyServiceId",
                        column: x => x.CompanyServiceId,
                        principalSchema: "Company",
                        principalTable: "CompanyService",
                        principalColumn: "CompanyServiceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyServiceUserRole_CompanyServiceRole_CompanyServiceRoleId",
                        column: x => x.CompanyServiceRoleId,
                        principalSchema: "Company",
                        principalTable: "CompanyServiceRole",
                        principalColumn: "CompanyServiceRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyServiceUserRole_CompanyServiceUser_CompanyServiceUserId",
                        column: x => x.CompanyServiceUserId,
                        principalSchema: "Company",
                        principalTable: "CompanyServiceUser",
                        principalColumn: "CompanyServiceUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Company",
                table: "Guest",
                columns: new[] { "GuestId", "AddressLine", "CUCC", "CityId", "CityName", "CompanyId", "CountryId", "CountryName", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "EmailAddress", "FaxNumber", "FaxNumberPrefix", "FaxNumberPrefixId", "FirstName", "GuestName", "GuestType", "Id", "Image", "IsDeleted", "IsGuest", "LastName", "MobileNumber", "MobileNumberPrefix", "MobileNumberPrefixId", "ModifiedBy", "ModifiedOn", "PhoneNumber", "PhoneNumberPrefix", "PhoneNumberPrefixId", "StateId", "StateName", "ZipCode" },
                values: new object[] { 1, "None", null, "None", "None", 1, 1, "Global", 0, new DateTime(2020, 10, 8, 9, 55, 31, 520, DateTimeKind.Utc).AddTicks(3012), 0, new DateTime(2020, 10, 8, 9, 55, 31, 521, DateTimeKind.Utc).AddTicks(3195), "None", "None", "None", "None", "None", "None", 0, new Guid("7bbe1f11-e04a-4a8b-afe1-ec55eba13d66"), null, (byte)1, false, "None", "None", "None", "None", 0, new DateTime(2020, 10, 8, 9, 55, 31, 520, DateTimeKind.Utc).AddTicks(7058), "None", "None", "None", "None", "None", "None" });

            migrationBuilder.InsertData(
                schema: "General",
                table: "ContactDetail",
                columns: new[] { "ContactDetailId", "CreatedBy", "CreatedOn", "Fax", "FaxPrefix", "FaxPrefixId", "Guid", "IsDeleted", "Mobile", "MobilePrefix", "MobilePrefixId", "ModifiedBy", "ModifiedOn", "Phone", "PhonePrefix", "PhonePrefixId" },
                values: new object[] { 1, 0, new DateTime(2020, 10, 8, 9, 55, 31, 510, DateTimeKind.Utc).AddTicks(4294), "None", "None", 1, new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), (byte)1, "None", "None", 1, 0, new DateTime(2020, 10, 8, 9, 55, 31, 510, DateTimeKind.Utc).AddTicks(8924), "None", "None", 1 });

            migrationBuilder.InsertData(
                schema: "Settings",
                table: "AddressType",
                columns: new[] { "AddressTypeId", "CreatedBy", "CreatedOn", "Guid", "IsDeleted", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2020, 10, 8, 9, 55, 31, 496, DateTimeKind.Utc).AddTicks(9910), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), (byte)0, 0, new DateTime(2020, 10, 8, 9, 55, 31, 497, DateTimeKind.Utc).AddTicks(3533), "Company" },
                    { 2, 0, new DateTime(2020, 10, 8, 9, 55, 31, 504, DateTimeKind.Utc).AddTicks(1232), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), (byte)0, 0, new DateTime(2020, 10, 8, 9, 55, 31, 504, DateTimeKind.Utc).AddTicks(1341), "Residential" }
                });

            migrationBuilder.InsertData(
                schema: "General",
                table: "Address",
                columns: new[] { "AddressId", "AddressLine", "AddressTypeId", "CityId", "CityName", "CountryId", "CountryName", "CreatedBy", "CreatedOn", "FullAddress", "Guid", "IsDeleted", "Latitude", "Longitude", "ModifiedBy", "ModifiedOn", "StateId", "StateName", "ZipCode" },
                values: new object[] { 1, "None", 1, 1, "None", 1, "Global", 0, new DateTime(2020, 10, 8, 9, 55, 31, 506, DateTimeKind.Utc).AddTicks(5719), "None", new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), (byte)1, "None", "None", 0, new DateTime(2020, 10, 8, 9, 55, 31, 507, DateTimeKind.Utc).AddTicks(350), 1, "None", "None" });

            migrationBuilder.InsertData(
                schema: "Company",
                table: "Company",
                columns: new[] { "CompanyId", "AccreditedBy", "AddressId", "CUCC", "ClientId", "CompanyCode", "CompanyName", "ContactDetailId", "CreatedBy", "CreatedOn", "EmailAddress", "Guid", "ImageURL", "IsDeleted", "ModifiedBy", "ModifiedOn", "Status", "TaxExemption", "TaxExemptionStatus", "WebsiteURL" },
                values: new object[] { 1, null, 1, null, 1, null, "None", 1, 0, new DateTime(2020, 10, 8, 9, 55, 31, 514, DateTimeKind.Utc).AddTicks(4411), "None", new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), "None", (byte)1, 0, new DateTime(2020, 10, 8, 9, 55, 31, 514, DateTimeKind.Utc).AddTicks(9023), (byte)0, (byte)0, (byte)0, "None" });

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
                name: "IX_CompanyGroupResource_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyGroupResource",
                column: "CompanyServiceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyService_CompanyId",
                schema: "Company",
                table: "CompanyService",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceRole_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRole",
                column: "CompanyServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUser_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUser_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUser_CompanyUserId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUserRole_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUserRole_CompanyServiceRoleId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceUserRole_CompanyServiceUserId",
                schema: "Company",
                table: "CompanyServiceUserRole",
                column: "CompanyServiceUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_CompanyId",
                schema: "Company",
                table: "CompanyUser",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_UserId",
                schema: "Company",
                table: "CompanyUser",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invite_CompanyId",
                schema: "Company",
                table: "Invite",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_AddressTypeId",
                schema: "General",
                table: "Address",
                column: "AddressTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ContactDetailId",
                schema: "Users",
                table: "User",
                column: "ContactDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyGroupResource",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "CompanyServiceUserRole",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "Guest",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "Invite",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "PreferredContact",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "PreferredProvider",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "AuditLog",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "CompanyServiceUser",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "CompanyServiceRole",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "CompanyUser",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "CompanyService",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Users");

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
