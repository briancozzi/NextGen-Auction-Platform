using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.BiddingPlatform.Migrations
{
    public partial class added_appaccountid_to_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppAccountId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_AppAccountId",
                table: "AbpUsers",
                column: "AppAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_AppAccounts_AppAccountId",
                table: "AbpUsers",
                column: "AppAccountId",
                principalTable: "AppAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_AppAccounts_AppAccountId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_AppAccountId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "AppAccountId",
                table: "AbpUsers");
        }
    }
}
