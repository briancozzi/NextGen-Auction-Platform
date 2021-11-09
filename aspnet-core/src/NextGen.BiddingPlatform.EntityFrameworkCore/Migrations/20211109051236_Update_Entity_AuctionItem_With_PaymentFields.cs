using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.BiddingPlatform.Migrations
{
    public partial class Update_Entity_AuctionItem_With_PaymentFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "AuctionItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentStatusUpdateDate",
                table: "AuctionItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "AuctionItems");

            migrationBuilder.DropColumn(
                name: "PaymentStatusUpdateDate",
                table: "AuctionItems");
        }
    }
}
