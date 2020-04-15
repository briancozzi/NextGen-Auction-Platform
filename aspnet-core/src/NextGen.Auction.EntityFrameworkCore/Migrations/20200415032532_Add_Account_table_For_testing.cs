using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.Auction.Migrations
{
    public partial class Add_Account_table_For_testing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    CountryCodeForMobile = table.Column<string>(nullable: true),
                    MobileNo = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IsEmailVerified = table.Column<bool>(nullable: false),
                    IsMobileVerified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
