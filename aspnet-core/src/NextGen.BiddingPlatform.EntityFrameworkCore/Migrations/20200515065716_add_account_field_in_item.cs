using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.BiddingPlatform.Migrations
{
    public partial class add_account_field_in_item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppAccountId",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_AppAccountId",
                table: "Items",
                column: "AppAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AppAccounts_AppAccountId",
                table: "Items",
                column: "AppAccountId",
                principalTable: "AppAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AppAccounts_AppAccountId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_AppAccountId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AppAccountId",
                table: "Items");
        }
    }
}
