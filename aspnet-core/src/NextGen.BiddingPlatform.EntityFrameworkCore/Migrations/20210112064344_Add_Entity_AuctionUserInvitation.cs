using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.BiddingPlatform.Migrations
{
    public partial class Add_Entity_AuctionUserInvitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuctionLink",
                table: "Auctions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuctionUserInvitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    AuctionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionUserInvitations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuctionUserInvitations");

            migrationBuilder.DropColumn(
                name: "AuctionLink",
                table: "Auctions");
        }
    }
}
