using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.BiddingPlatform.Migrations
{
    public partial class Updated_Entity_AuctionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOutBid",
                table: "AuctionHistory",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOutBid",
                table: "AuctionHistory");
        }
    }
}
