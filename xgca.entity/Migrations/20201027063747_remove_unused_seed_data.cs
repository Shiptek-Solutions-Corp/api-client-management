using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class remove_unused_seed_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 10, 27, 6, 37, 46, 96, DateTimeKind.Utc).AddTicks(5368), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 10, 27, 6, 37, 46, 96, DateTimeKind.Utc).AddTicks(7262) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 10, 27, 6, 37, 46, 99, DateTimeKind.Utc).AddTicks(3869), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 10, 27, 6, 37, 46, 99, DateTimeKind.Utc).AddTicks(3921) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 10, 27, 6, 34, 13, 427, DateTimeKind.Utc).AddTicks(2774), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 10, 27, 6, 34, 13, 427, DateTimeKind.Utc).AddTicks(3756) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 10, 27, 6, 34, 13, 428, DateTimeKind.Utc).AddTicks(6028), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 10, 27, 6, 34, 13, 428, DateTimeKind.Utc).AddTicks(6046) });
        }
    }
}
