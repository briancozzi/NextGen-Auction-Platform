using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.BiddingPlatform.Migrations
{
    public partial class update_event_account_entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "AppAccountEvents");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailImage",
                table: "AppAccounts",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Addresses",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionHistory_AuctionItemId",
                table: "AuctionHistory",
                column: "AuctionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionBidders_AuctionId",
                table: "AuctionBidders",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionBidders_Auctions_AuctionId",
                table: "AuctionBidders",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionHistory_AuctionItems_AuctionItemId",
                table: "AuctionHistory",
                column: "AuctionItemId",
                principalTable: "AuctionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionBidders_Auctions_AuctionId",
                table: "AuctionBidders");

            migrationBuilder.DropForeignKey(
                name: "FK_AuctionHistory_AuctionItems_AuctionItemId",
                table: "AuctionHistory");

            migrationBuilder.DropIndex(
                name: "IX_AuctionHistory_AuctionItemId",
                table: "AuctionHistory");

            migrationBuilder.DropIndex(
                name: "IX_AuctionBidders_AuctionId",
                table: "AuctionBidders");

            migrationBuilder.DropColumn(
                name: "ThumbnailImage",
                table: "AppAccounts");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "AppAccountEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Addresses",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
