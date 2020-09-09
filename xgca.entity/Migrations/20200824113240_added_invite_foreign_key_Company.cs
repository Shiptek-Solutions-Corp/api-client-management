using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_invite_foreign_key_Company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GuestId",
                schema: "Company",
                table: "PreferredContact",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                schema: "Company",
                table: "PreferredContact",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValue: new Guid("aa2ce8c8-b78d-45b3-85f5-d8d63c4858db"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("20ab4a8f-876f-4a3d-839e-537ba35cb549"));

            migrationBuilder.AddColumn<string>(
                name: "UCCCode",
                schema: "Company",
                table: "Company",
                nullable: true);

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

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 24, 11, 32, 40, 53, DateTimeKind.Utc).AddTicks(9212), new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), new DateTime(2020, 8, 24, 11, 32, 40, 54, DateTimeKind.Utc).AddTicks(379) });

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 24, 11, 32, 40, 56, DateTimeKind.Utc).AddTicks(1790), new DateTime(2020, 8, 24, 11, 32, 40, 56, DateTimeKind.Utc).AddTicks(4715), new DateTime(2020, 8, 24, 11, 32, 40, 56, DateTimeKind.Utc).AddTicks(2978) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 24, 11, 32, 40, 51, DateTimeKind.Utc).AddTicks(6300), new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), new DateTime(2020, 8, 24, 11, 32, 40, 51, DateTimeKind.Utc).AddTicks(7502) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "ContactDetail",
                keyColumn: "ContactDetailId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 24, 11, 32, 40, 52, DateTimeKind.Utc).AddTicks(9297), new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), new DateTime(2020, 8, 24, 11, 32, 40, 53, DateTimeKind.Utc).AddTicks(719) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 24, 11, 32, 40, 48, DateTimeKind.Utc).AddTicks(6244), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 8, 24, 11, 32, 40, 48, DateTimeKind.Utc).AddTicks(8303) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 24, 11, 32, 40, 50, DateTimeKind.Utc).AddTicks(5340), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 8, 24, 11, 32, 40, 50, DateTimeKind.Utc).AddTicks(5383) });

            migrationBuilder.CreateIndex(
                name: "IX_Invite_CompanyId",
                schema: "Company",
                table: "Invite",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invite",
                schema: "Company");

            migrationBuilder.DropColumn(
                name: "UCCCode",
                schema: "Company",
                table: "Company");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                schema: "Company",
                table: "PreferredContact",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                schema: "Company",
                table: "PreferredContact",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("20ab4a8f-876f-4a3d-839e-537ba35cb549"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("aa2ce8c8-b78d-45b3-85f5-d8d63c4858db"));

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 19, 18, 18, 34, 33, DateTimeKind.Utc).AddTicks(4749), new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), new DateTime(2020, 8, 19, 18, 18, 34, 33, DateTimeKind.Utc).AddTicks(6599) });

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 19, 18, 18, 34, 36, DateTimeKind.Utc).AddTicks(9122), new DateTime(2020, 8, 19, 18, 18, 34, 37, DateTimeKind.Utc).AddTicks(3836), new DateTime(2020, 8, 19, 18, 18, 34, 37, DateTimeKind.Utc).AddTicks(1138) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 19, 18, 18, 34, 30, DateTimeKind.Utc).AddTicks(1956), new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), new DateTime(2020, 8, 19, 18, 18, 34, 30, DateTimeKind.Utc).AddTicks(3814) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "ContactDetail",
                keyColumn: "ContactDetailId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 19, 18, 18, 34, 31, DateTimeKind.Utc).AddTicks(9550), new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), new DateTime(2020, 8, 19, 18, 18, 34, 32, DateTimeKind.Utc).AddTicks(1470) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 19, 18, 18, 34, 25, DateTimeKind.Utc).AddTicks(6593), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 8, 19, 18, 18, 34, 25, DateTimeKind.Utc).AddTicks(9639) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 19, 18, 18, 34, 28, DateTimeKind.Utc).AddTicks(7050), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 8, 19, 18, 18, 34, 28, DateTimeKind.Utc).AddTicks(7092) });
        }
    }
}
