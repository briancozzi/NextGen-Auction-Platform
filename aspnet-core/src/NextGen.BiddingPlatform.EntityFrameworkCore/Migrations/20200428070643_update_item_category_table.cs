using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.BiddingPlatform.Migrations
{
    public partial class update_item_category_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ItemCategories",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "ItemCategories",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "ItemCategories",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "ItemCategories",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UniqueId",
                table: "ItemCategories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategory_UniqueId",
                table: "ItemCategories",
                column: "UniqueId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemCategory_UniqueId",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "ItemCategories");
        }
    }
}
