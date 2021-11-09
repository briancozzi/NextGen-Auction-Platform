using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.BiddingPlatform.Migrations
{
    public partial class Updated_Entity_AuctionItem_With_WinnerBidderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WinnerBidderId",
                table: "AuctionItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionItems_WinnerBidderId",
                table: "AuctionItems",
                column: "WinnerBidderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionItems_AuctionBidders_WinnerBidderId",
                table: "AuctionItems",
                column: "WinnerBidderId",
                principalTable: "AuctionBidders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionItems_AuctionBidders_WinnerBidderId",
                table: "AuctionItems");

            migrationBuilder.DropIndex(
                name: "IX_AuctionItems_WinnerBidderId",
                table: "AuctionItems");

            migrationBuilder.DropColumn(
                name: "WinnerBidderId",
                table: "AuctionItems");
        }
    }
}
