using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.BiddingPlatform.Migrations
{
    public partial class changes_type_in_event : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventEndDateTime",
                table: "AppAccountEvents");

            migrationBuilder.DropColumn(
                name: "EventStartDateTime",
                table: "AppAccountEvents");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EventEndTime",
                table: "AppAccountEvents",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EventStartTime",
                table: "AppAccountEvents",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

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
                name: "EventEndTime",
                table: "AppAccountEvents");

            migrationBuilder.DropColumn(
                name: "EventStartTime",
                table: "AppAccountEvents");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventEndDateTime",
                table: "AppAccountEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EventStartDateTime",
                table: "AppAccountEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
