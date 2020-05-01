using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.BiddingPlatform.Migrations
{
    public partial class Event_Entity_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "EventDate",
            //    table: "AppAccountEvents");

            migrationBuilder.DropColumn(
                name: "EventEndTime",
                table: "AppAccountEvents");

            migrationBuilder.DropColumn(
                name: "EventStartTime",
                table: "AppAccountEvents");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventEndDateTime",
                table: "AppAccountEvents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EventStartDateTime",
                table: "AppAccountEvents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Addresses",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

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
                name: "EventEndDateTimee",
                table: "AppAccountEvents");

            migrationBuilder.DropColumn(
                name: "EventStartDateTimee",
                table: "AppAccountEvents");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "AppAccountEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
